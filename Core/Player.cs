using OpenTK.Mathematics;

using GameEngine.Core.Rendering;
using GameEngine.Core.Terrain;
using GameEngine.Core.Utilities;
using OpenTK.Graphics.OpenGL4;
using GameEngine.Core.Utilities.Managers;

namespace GameEngine.Core {
    public class Player {
        public const float gravity = 9.81f;

        public Vector3 Position { get; set; }
        public Vector2 ChunkPos { get; private set; }

        public Camera cam;
        private TerrainModifier modifier;
        private TextureAtlas atlas;

        private VertexBuffer vertexBuffer;
        private VertexArray vertexArray;
        private IndexBuffer indexBuffer;
        private BufferLayout layout;

        public Player(Vector3 position, TextureAtlas atlas) {
            cam = new Camera(position, 10f, 0.2f, 0.1f, 1000f);
            modifier = new TerrainModifier(cam);
            this.atlas = atlas;

            ChunkPos = new Vector2((float)MathHelper.Floor(cam.Position.X / 16f), (float)MathHelper.Floor(cam.Position.Z / 16f));

            layout = new BufferLayout();

            // specific for this cube
            layout.Add<float>(3); // vertices
            layout.Add<float>(2); // texture coords
            layout.Add<float>(1);

            vertexArray = new VertexArray();
            vertexBuffer = new VertexBuffer(Crosshair(Vector3.Zero));
            indexBuffer = new IndexBuffer(new uint[6] {0, 1, 2, 0, 2, 3});

            vertexArray.AddBuffer(vertexBuffer, layout);
        }

        public void Update(float dt) {
            cam.MoveCamera(dt);
            modifier.Update(dt);

            ChunkPos = new Vector2((float)MathHelper.Floor(cam.Position.X / 16f), (float)MathHelper.Floor(cam.Position.Z / 16f));
        }

        public void Draw() {
            vertexArray.Bind();
            indexBuffer.Bind();
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        private float[] Crosshair(Vector3 pos) {
            Vector2[] a = atlas.GetTextureCoords(TextureAtlas.BlockType.UI_Crosshair, TextureAtlas.BlockFace.Side);

            float[] face = {
                0.5f, -0.5f, 0.5f,          a[0].X, a[0].Y,     1f, // bottom right
                0.5f, 0.5f, 0.5f,           a[1].X, a[1].Y,     1f, // top right
                -0.5f, 0.5f, 0.5f,          a[2].X, a[2].Y,     1f, // top left
                -0.5f, -0.5f, 0.5f,         a[3].X, a[3].Y,     1f, // bottom left
            };

            return face;
        }
    }
}
