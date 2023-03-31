using GameEngine.Core.Rendering;
using GameEngine.Core.Utilities.Managers;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Core.Terrain {
    public class TerrainModifier {
        public const int PlayerReach = 5;

        private int blockSelected;

        private Camera cam;

        private float nextPlace;
        private const float placeIntervall = 0.1f;

        public TerrainModifier(Camera cam) {
            this.cam = cam;
            blockSelected = (int)TextureAtlas.BlockType.Wood;
        }

        private void SelectBlock() {

        }

        public void Update(float dt) {
            nextPlace -= dt;

            if ((Input.MousePress(MouseButton.Left) || Input.MousePress(MouseButton.Right)) && nextPlace <= 0) {
                nextPlace = placeIntervall;
                if (Raycast.InterceptTerrain(cam.Position, cam.Front, PlayerReach, 0.1f, out RaycastInfo info) == false) return;

                Vector3 p;

                if (Input.MousePress(MouseButton.Right)) { p = info.point - info.rayDirection * 0.06f; } // place 
                else { p = info.point + info.rayDirection * 0.02f; } // break

                // check if target place block is air

                Vector2 chunkPos = new Vector2((float)MathHelper.Floor(p.X / 16f), (float)MathHelper.Floor(p.Z / 16f));
                Chunk chunk = TerrainGenerator.loadedChunks[chunkPos];

                int bX = (int)MathHelper.Floor(p.X) - (int)chunkPos.X * 16;
                int bY = (int)MathHelper.Floor(p.Y);
                int bZ = (int)MathHelper.Floor(p.Z) - (int)chunkPos.Y * 16;

                if (Input.MousePress(MouseButton.Left)) {
                    chunk.Modify(new Vector3i(bX, bY, bZ), TextureAtlas.BlockType.Air);
                } else {
                    chunk.Modify(new Vector3i(bX, bY, bZ), TextureAtlas.BlockType.Brick);
                }

                chunk.BuildMesh();
            }
        }
    }
}
