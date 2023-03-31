using GameEngine.Core.Terrain;
using OpenTK.Mathematics;

namespace GameEngine.Core
{
    public class Raycast {
        public Vector3 currentRay { get; private set; }

        public static RaycastInfo InterceptTerrain(Vector3 origin, Vector3 direction, float maxDistance, float stepSize) {
            Vector3 rayWorldPos = Vector3.Zero;

            float step = 0f;
            while (rayWorldPos.Length <= origin.Length + maxDistance) {
                Vector3 ray = new Vector3(direction.X * step, direction.Y * step, direction.Z * step);
                rayWorldPos = Vector3.Add(origin, ray);

                Vector2 chunkPos = new Vector2((float)MathHelper.Floor(rayWorldPos.X / 16f), (float)MathHelper.Floor(rayWorldPos.Z / 16f));


                int bX = (int) MathHelper.Floor(rayWorldPos.X) - ((int)chunkPos.X * 16);
                int bY = (int) MathHelper.Floor(rayWorldPos.Y);
                int bZ = (int) MathHelper.Floor(rayWorldPos.Z) - ((int)chunkPos.Y * 16);

                if (TerrainGenerator.loadedChunks[chunkPos].data[bX, MathHelper.Clamp(bY, 0, Chunk.Height), bZ].blockType != Rendering.TextureAtlas.BlockType.Air) {
                    return new RaycastInfo(point: rayWorldPos, distance: MathHelper.Abs((rayWorldPos - origin).Length), rayDirection: direction); ; 
                }

                step += stepSize;
            }
            return new RaycastInfo();
        }

        public static bool InterceptTerrain(Vector3 origin, Vector3 direction, float maxDistance, float stepSize, out RaycastInfo info) {
            Vector3 rayWorldPos = Vector3.Zero;

            float step = 0f;
            while (rayWorldPos.Length <= origin.Length + maxDistance) {
                Vector3 ray = new Vector3(direction.X * step, direction.Y * step, direction.Z * step);
                rayWorldPos = Vector3.Add(origin, ray);

                Vector2 chunkPos = new Vector2((float)MathHelper.Floor(rayWorldPos.X / 16f), (float)MathHelper.Floor(rayWorldPos.Z / 16f));


                int bX = (int)MathHelper.Floor(rayWorldPos.X) - ((int)chunkPos.X * 16);
                int bY = (int)MathHelper.Floor(rayWorldPos.Y);
                int bZ = (int)MathHelper.Floor(rayWorldPos.Z) - ((int)chunkPos.Y * 16);

                if (TerrainGenerator.loadedChunks[chunkPos].data[bX, MathHelper.Clamp(bY, 0, Chunk.Height), bZ].blockType != Rendering.TextureAtlas.BlockType.Air) {

                    info = new RaycastInfo(
                        point: rayWorldPos,
                        distance: MathHelper.Abs((rayWorldPos - origin).Length),
                        rayDirection: direction
                        );

                    return true;
                }

                step += stepSize;
            }

            info = new RaycastInfo();
            return false;
        }



        //public void Update(Vector2 mouseCoords) {
        //    viewMatrix = camera.GetViewMatrix();
        //    currentRay = calculateMouseRay(mouseCoords.X, mouseCoords.Y);
        //}

        //private Vector3 calculateMouseRay(float mouseX, float mouseY) {
        //    Vector2 normalizedCoords = getNormalisedDeviceCoordinates(mouseX, mouseY);
        //    Vector4 clipCoords = new Vector4(normalizedCoords.X, normalizedCoords.Y, -1.0f, 1.0f);
        //    Vector4 eyeCoords = toEyeCoords(clipCoords);
        //    Vector3 worldRay = toWorldCoords(eyeCoords);
        //    return worldRay;
        //}

        //private Vector3 toWorldCoords(Vector4 eyeCoords) {
        //    Matrix4 invertedView = Matrix4.Invert(viewMatrix);
        //    Vector4 rayWorld = invertedView * eyeCoords;
        //    Vector3 mouseRay = new Vector3(rayWorld.X, rayWorld.Y, rayWorld.Z);
        //    mouseRay.Normalize();
        //    return mouseRay;
        //}

        //private Vector4 toEyeCoords(Vector4 clipCoords) {
        //    Matrix4 invertedProjection = Matrix4.Invert(projectionMatrix);
        //    Vector4 eyeCoords = invertedProjection * clipCoords;
        //    return new Vector4(eyeCoords.X, eyeCoords.Y, -1f, 0f);
        //}

        //private Vector2 getNormalisedDeviceCoordinates(float mouseX, float mouseY) {
        //    float x = (2.0f * mouseX) / Game.windowSize.X - 1f;
        //    float y = (2.0f * mouseY) / Game.windowSize.Y - 1f;
        //    return new Vector2(x, y);
        //}

    }

    public struct RaycastInfo {
        public Vector3 point;
        public float distance;
        public Vector3 rayDirection;

        public RaycastInfo(Vector3 point, float distance, Vector3 rayDirection) {
            this.point = point;
            this.distance = distance;
            this.rayDirection = rayDirection;
        }
    }
}
