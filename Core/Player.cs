using OpenTK.Mathematics;

using GameEngine.Core.Rendering;
using GameEngine.Core.Terrain;

namespace GameEngine.Core {
    public class Player {
        public const float gravity = 9.81f;

        public Vector3 Position { get; set; }
        public Vector2 ChunkPos { get; private set; }

        public Camera cam;
        private TerrainModifier modifier;

        public Player(Vector3 position) {
            cam = new Camera(position, 10f, 0.2f, 0.1f, 10000f);
            modifier = new TerrainModifier(cam);

            ChunkPos = new Vector2((float)MathHelper.Floor(cam.Position.X / 16f), (float)MathHelper.Floor(cam.Position.Z / 16f));
        }

        public void Update(float dt) {
            cam.MoveCamera(dt);

            //cam.Position -= Vector3.UnitY * (gravity * dt);

            modifier.Update(dt);

            ChunkPos = new Vector2((float)MathHelper.Floor(cam.Position.X / 16f), (float)MathHelper.Floor(cam.Position.Z / 16f));

            //int bX = MathHelper.Clamp((int)MathHelper.Floor(cam.Position.X) - ((int)ChunkPos.X * 16), 0, Chunk.Width-1);
            //int bY = MathHelper.Clamp((int)MathHelper.Floor(cam.Position.Y - 2), 0, Chunk.Height - 1);
            //int bZ = MathHelper.Clamp((int)MathHelper.Floor(cam.Position.Z) - ((int)ChunkPos.Y * 16), 0, Chunk.Width-1);

            //if (TerrainGenerator.loadedChunks[ChunkPos].data[bX, bY, bZ] == 1) {
            //    //cam.Position += Vector3.UnitY * (gravity * dt);
            //}

        }
    }
}
