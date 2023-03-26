using GameEngine.Core.Rendering;
using GameEngine.Core.Utilities;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace GameEngine.Core.Terrain {
    public class Chunk : IDisposable {
        public const int Width = 16;
        public const int Height = 32;
        public const float LightFallOff = 0.08f;

        public byte[,,] data;
        private float[,,] lightData;
        // que em
        public Queue<BlockModification> modifications = new Queue<BlockModification>();

        public int chunkX { get; private set; }
        public int chunkZ { get; private set; }
        public bool isActive;
        public int highestPoint = 0;

        private List<float> vertices;
        private List<uint> indicesList;
        private int indicesLength;

        private VertexBuffer vertexBuffer;
        private VertexArray vertexArray;
        private IndexBuffer indexBuffer;
        private BufferLayout layout;

        private TextureAtlas atlas;

        public Chunk(Vector2 pos, byte[,,] data, TextureAtlas atlas) {
            chunkX = (int)pos.X;
            chunkZ = (int)pos.Y;

            this.atlas = atlas;
            this.data = data;

            vertices = new List<float>();
            indicesList = new List<uint>();

            layout = new BufferLayout();

            // specific for this cube
            layout.Add<float>(3); // vertices
            layout.Add<float>(2); // texture coords
            layout.Add<float>(1);

            vertexArray = new VertexArray();
            vertexBuffer = new VertexBuffer();
            indexBuffer = new IndexBuffer();

            lightData = new float[Chunk.Width + 1, Chunk.Height + 1, Chunk.Width + 1];

            BuildMesh();
        }

        ~Chunk() {
            Dispose();
        }

        public void Dispose() {
            vertexArray.Unbind();
            indexBuffer.Unbind();
            vertexBuffer.Unbind();

            vertices = null;
            indicesList = null;

            GC.SuppressFinalize(this);
        }

        public ChunkDrawData ReturnChunkDrawData() {
            return new ChunkDrawData(vertices, indicesList);
        }

        // maybe combine all draw calls into one
        public void Draw() {
            vertexArray.Bind();
            indexBuffer.Bind();
            GL.DrawElements(PrimitiveType.Triangles, indicesLength, DrawElementsType.UnsignedInt, 0);
        }

        public void Modify(Vector3i pos, TextureAtlas.BlockType type) {
            modifications.Enqueue(new BlockModification(pos, type));
        }


        void CalculateLight() {

            Queue<Vector3i> litVoxels = new Queue<Vector3i>();

            for (int x = 0; x < Width; x++) {
                for (int z = 0; z < Width; z++) {

                    // "ray" casted from the sun straight down. blocks reduce its intensity
                    float lightRay = 1f;

                    for (int y = Height - 1; y >= 0; y--) {
                        if (data[x, y, z] != 0) {
                            float trans = TextureAtlas.TexturePositions[(TextureAtlas.BlockType)data[x, y, z]].transparencyAmount;
                            if (trans < lightRay) { lightRay = trans; }
                        }

                        lightData[x, y, z] = lightRay;

                        if (lightRay > LightFallOff) { litVoxels.Enqueue(new Vector3i(x, y, z)); }
                    }
                }
            }

            while (litVoxels.Count > 0) {

                Vector3i v = litVoxels.Dequeue();

                for (int p = 0; p < 6; p++) {

                    Vector3i currentVoxel = v + TextureAtlas.faceChecks[p]; // check all faces
                    Vector3i neighbor = new Vector3i(currentVoxel.X, currentVoxel.Y, currentVoxel.Z);

                    if (IsBlockInChunk(neighbor.X, neighbor.Y, neighbor.Z) == false) { continue; }

                    if (lightData[neighbor.X, neighbor.Y, neighbor.Z] > lightData[v.X, v.Y, v.Z] - LightFallOff) { continue; }


                    lightData[neighbor.X, neighbor.Y, neighbor.Z] = Math.Clamp(lightData[v.X, v.Y, v.Z] - LightFallOff, 0.1f, 1f);

                    if (lightData[neighbor.X, neighbor.Y, neighbor.Z] > LightFallOff) { litVoxels.Enqueue(neighbor); }

                }

            }

        }

        public void BuildMesh() {
            vertexArray.RemoveBuffer(vertexBuffer);
            vertexArray?.Unbind();
            indexBuffer?.Unbind();
            vertexBuffer?.Unbind();

            vertices.Clear();
            indicesList.Clear();

            while (modifications.Count > 0) {
                BlockModification mod = modifications.Dequeue();
                data[mod.blockPosition.X, mod.blockPosition.Y, mod.blockPosition.Z] = (byte)mod.blockType;
            }

            CalculateLight();


            for (int x = 0; x < Width; x++) {
                for (int z = 0; z < Width; z++) {
                    for (int y = 0; y < Height; y++) {

                        if (data[x, y, z] == 0) continue;
                        if (highestPoint < y) { highestPoint = y; }

                        Vector3 pos = new Vector3(x + chunkX * (Width - 1), y, z + chunkZ * (Width - 1));

                        TextureAtlas.BlockType b = (TextureAtlas.BlockType)data[x, y, z];
                        float light = lightData[x, y, z];

                        if (data[x, y + 1, z] == (int)TextureAtlas.BlockType.Air) {
                            TopFace(pos, b, light);
                        }
                        if (data[x, MathHelper.Max(y - 1, 0), z] == (int)TextureAtlas.BlockType.Air) {
                            BottomFace(pos, b, light);
                        }
                        if (data[x + 1, y, z] == (int)TextureAtlas.BlockType.Air) {
                            RightFace(pos, b, light);
                        }
                        if (x - 1 < 0 || data[x - 1, y, z] == (int)TextureAtlas.BlockType.Air) {
                            LeftFace(pos, b, light);
                        }
                        if (z - 1 < 0 || data[x, y, z - 1] == (int)TextureAtlas.BlockType.Air) {
                            BackFace(pos, b, light);
                        }
                        if (data[x, y, z + 1] == (int)TextureAtlas.BlockType.Air) {
                            FrontFace(pos, b, light);
                        }

                    }
                }
            }

            // create indicies
            for (uint i = 0; i < vertices.Count; i += 4) {
                // 4 vertecies for plane; 6 indecies
                indicesList.Add(i);
                indicesList.Add(i + 1);
                indicesList.Add(i + 2);
                indicesList.Add(i);
                indicesList.Add(i + 2);
                indicesList.Add(i + 3);
            }

            indicesLength = indicesList.Count;

            vertexBuffer.LoadData(vertices.ToArray(), BufferUsageHint.StreamDraw);
            indexBuffer.LoadData(indicesList.ToArray(), BufferUsageHint.StreamDraw);

            vertexArray.AddBuffer(vertexBuffer, layout);
        }

        private bool IsBlockInChunk(int x, int y, int z) {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1 || z < 0 || z > Width - 1)
                return false;
            else
                return true;
        }

        private void TopFace(Vector3 pos, TextureAtlas.BlockType block, float brightness) {
            Vector2[] a = atlas.GetTextureCoords(block, TextureAtlas.BlockFace.Top);

            float[] face = {
                pos.X, pos.Y + 1f, pos.Z,                   a[0].X, a[0].Y,     brightness,
                pos.X, pos.Y + 1f, pos.Z + 1f,              a[1].X, a[1].Y,     brightness,
                pos.X + 1f, pos.Y + 1f, pos.Z + 1f,         a[2].X, a[2].Y,     brightness,
                pos.X + 1f, pos.Y + 1f, pos.Z,              a[3].X, a[3].Y,     brightness,
            };

            vertices.AddRange(face);
        }
        private void BottomFace(Vector3 pos, TextureAtlas.BlockType block, float brightness) {
            Vector2[] a = atlas.GetTextureCoords(block, TextureAtlas.BlockFace.Bottom);

            float[] face = {
                pos.X, pos.Y, pos.Z,                a[0].X, a[0].Y,     brightness,
                pos.X + 1f, pos.Y, pos.Z,           a[1].X, a[1].Y,     brightness,
                pos.X + 1f, pos.Y, pos.Z + 1f,      a[2].X, a[2].Y,     brightness,
                pos.X, pos.Y, pos.Z + 1f,                a[3].X, a[3].Y,     brightness,
            };

            vertices.AddRange(face);
        }
        private void FrontFace(Vector3 pos, TextureAtlas.BlockType block, float brightness) {
            Vector2[] a = atlas.GetTextureCoords(block, TextureAtlas.BlockFace.Side);

            float[] face = {
                pos.X + 1f, pos.Y, pos.Z + 1f,          a[0].X, a[0].Y,     brightness, // bottom right
                pos.X + 1f, pos.Y + 1f, pos.Z + 1f,     a[1].X, a[1].Y,     brightness, // top right
                pos.X, pos.Y + 1f, pos.Z + 1f,          a[2].X, a[2].Y,     brightness, // top left
                pos.X, pos.Y, pos.Z + 1f,               a[3].X, a[3].Y,     brightness, // bottom left
            };

            vertices.AddRange(face);
        }
        private void BackFace(Vector3 pos, TextureAtlas.BlockType block, float brightness) {
            Vector2[] a = atlas.GetTextureCoords(block, TextureAtlas.BlockFace.Side);

            float[] face = {
                pos.X, pos.Y, pos.Z,                a[0].X, a[0].Y,     brightness,
                pos.X, pos.Y + 1f, pos.Z,           a[1].X, a[1].Y,     brightness,
                pos.X + 1f, pos.Y + 1f, pos.Z,      a[2].X, a[2].Y,     brightness,
                pos.X + 1f, pos.Y, pos.Z,           a[3].X, a[3].Y,     brightness,
            };

            vertices.AddRange(face);
        }
        private void RightFace(Vector3 pos, TextureAtlas.BlockType block, float brightness) {
            Vector2[] a = atlas.GetTextureCoords(block, TextureAtlas.BlockFace.Side);

            float[] face = {
                pos.X + 1f, pos.Y, pos.Z,               a[0].X, a[0].Y,     brightness,
                pos.X + 1f, pos.Y + 1f, pos.Z,          a[1].X, a[1].Y,     brightness,
                pos.X + 1f, pos.Y + 1f, pos.Z + 1f,     a[2].X, a[2].Y,     brightness,
                pos.X + 1f, pos.Y, pos.Z + 1f,          a[3].X, a[3].Y,     brightness,
            };

            vertices.AddRange(face);
        }
        private void LeftFace(Vector3 pos, TextureAtlas.BlockType block, float brightness) {
            Vector2[] a = atlas.GetTextureCoords(block, TextureAtlas.BlockFace.Side);

            float[] face = {
                pos.X, pos.Y, pos.Z + 1f,         a[0].X, a[0].Y,   brightness,
                pos.X, pos.Y + 1f, pos.Z + 1f,    a[1].X, a[1].Y,   brightness,
                pos.X, pos.Y + 1f, pos.Z,         a[2].X, a[2].Y,   brightness,
                pos.X, pos.Y, pos.Z,              a[3].X, a[3].Y,   brightness,
            };

            vertices.AddRange(face);
        }

    }
}


public class ChunkDrawData {
    public List<float> vertices;
    public List<uint> indices;

    public ChunkDrawData(List<float> _vertices, List<uint> _indices) {
        vertices = _vertices;
        indices = _indices;
    }
}
