using GameEngine.Core.Rendering;
using OpenTK.Mathematics;

namespace GameEngine.Core.Terrain {
    public class BlockInfo {
        public Vector2 top;
        public Vector2 side;
        public Vector2 bottom;

        public float transparencyAmount;

        public BlockInfo(Vector2 t, Vector2 s, Vector2 b, float transparencyAmount = 0f) {
            top = t;
            side = s;
            bottom = b;
            this.transparencyAmount = transparencyAmount;
        }

        public BlockInfo(Vector2 face, float transparencyAmount = 0f) {
            top = side = bottom = face;
            this.transparencyAmount = transparencyAmount;
        }
    }

    public class BlockModification {
        public Vector3i blockPosition;
        public TextureAtlas.BlockType blockType;

        public BlockModification() {
            blockPosition = new Vector3i();
            blockType = 0;
        }

        public BlockModification(Vector3i _position, TextureAtlas.BlockType _blockType) {
            blockPosition = _position;
            blockType = _blockType;
        }
    }

    public class BlockState {
        public TextureAtlas.BlockType blockType;
        public float lightPercent;

        public BlockState(TextureAtlas.BlockType _blockType) { blockType = _blockType; }
    }

}
