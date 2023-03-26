using System.Linq;
using System.Collections.Generic;

using OpenTK.Mathematics;
using GameEngine.Core.Rendering;
using GameEngine.Core.Utilities.Managers;

namespace GameEngine.Core.Terrain {
    public class TerrainGenerator {
        public static Dictionary<Vector2, Chunk> loadedChunks;

        public TextureAtlas atlas { get; private set; }
        public int RenderDistance { get; private set; }
        public List<Chunk> chunkPool;

        private bool IsBatchingEnabled { get; set; }

        private Player player;
        private FastNoise noise;
        private TextureObject texture;


        public TerrainGenerator(TextureInfo info, int renderDistance, Player player) {
            texture = TextureFactory.Load(info.path);
            atlas = new TextureAtlas(info.resolution);

            this.player = player;
            RenderDistance = renderDistance;

            chunkPool = new List<Chunk>();
            loadedChunks = new Dictionary<Vector2, Chunk>();
            noise = new FastNoise(1337);

            SpawnChunks();
        }

        public TerrainGenerator(TextureInfo info, int renderDistance, Player player, bool batching) : this(info, renderDistance, player) {
            IsBatchingEnabled = batching;
        }



        public byte[,,] GetData(Vector2 vec) {
            return loadedChunks[vec].data;
        }

        public void UseTexture() {
            texture.Use();
        }

        public void BindAll() {
            Chunk[] chunks = loadedChunks.Values.ToArray();

            int i = 0;
            player.cam.CalculateFrustum();
            foreach (Chunk chunk in chunks) {
                Vector3 pos = new Vector3(chunk.chunkX * (Chunk.Width - 1), 0, chunk.chunkZ * (Chunk.Width - 1));

                if (!player.cam.frustum.VolumeVsFrustum(pos, Chunk.Width, Chunk.Height, Chunk.Width)) {
                    chunk.isActive = false;
                    continue;
                }
                i++;

                chunk.Draw();
            }
            DisplayManager.Instance.window.Title = $"{i} / {chunks.Length}"; //    { 1 / DisplayManager.Instance.window.UpdateTime }";
        }

        public static byte[,,] GetEmptyChunkList() {
            return new byte[Chunk.Width + 1, Chunk.Height + 1, Chunk.Width + 1];
        }

        public byte[,,] ChunkData(int chunkX, int chunkZ) {
            byte[,,] blocks = GetEmptyChunkList();

            for (int x = 0; x < Chunk.Width + 1; x++) {
                for (int z = 0; z < Chunk.Width + 1; z++) {
                    for (int y = 0; y < Chunk.Height + 1; y++) {
                        int xC = x + chunkX * Chunk.Width;
                        int zC = z + chunkZ * Chunk.Width;

                        if (noise.GetSimplex(xC, zC) * 10 + y < Chunk.Height * 0.5f) {
                            blocks[x, y, z] = (byte)TextureAtlas.BlockType.Grass;
                        }
                    }
                }
            }

            return blocks;
        }


        private void SpawnChunks() {
            for (int x = -RenderDistance; x <= RenderDistance; x++) {
                for (int y = -RenderDistance; y <= RenderDistance; y++) {
                    Vector2 v = new Vector2(x, y);
                    loadedChunks.Add(v, new Chunk(v, ChunkData((int)v.X, (int)v.Y), atlas));
                }
            }
        }
    }
}
