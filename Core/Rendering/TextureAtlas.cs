using OpenTK.Mathematics;
using System.Collections.Generic;

using GameEngine.Core.Terrain;

namespace GameEngine.Core.Rendering {

    public class TextureAtlas {
        private float resolution;

        public enum BlockType {
            Air,
            Grass,
            Timo,
            Asmir
        }
        public enum BlockFace {
            Top,
            Side,
            Bottom
        }

        public TextureAtlas(float resolution) {
            this.resolution = resolution;
        }

        public static Dictionary<BlockType, BlockInfo> TexturePositions = new Dictionary<BlockType, BlockInfo>() {
            { BlockType.Grass, new BlockInfo(new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2), 0f) }, // top, side, bottom
            { BlockType.Timo, new BlockInfo(new Vector2(0, 3), 0f) },
            { BlockType.Asmir, new BlockInfo(new Vector2(0, 4), 0f) }
        };

        public Vector2[] GetTextureCoords(BlockType block, BlockFace face = BlockFace.Top) {
            BlockInfo info = TexturePositions[block];

            switch (face) {
                case BlockFace.Top:
                    return new Vector2[] {
                        new Vector2((info.top.X+1)/resolution, info.top.Y/resolution),
                        new Vector2((info.top.X+1)/resolution, (info.top.Y+1)/resolution),
                        new Vector2(info.top.X/resolution, (info.top.Y+1)/resolution),
                        new Vector2(info.top.X/resolution, info.top.Y/resolution),

                    };
                case BlockFace.Side:
                    return new Vector2[] {
                        new Vector2((info.side.X+1)/resolution, info.side.Y/resolution),
                        new Vector2((info.side.X+1)/resolution, (info.side.Y+1)/resolution),
                        new Vector2(info.side.X/resolution, (info.side.Y+1)/resolution),
                        new Vector2(info.side.X/resolution, info.side.Y/resolution),
                    };
                case BlockFace.Bottom:
                    return new Vector2[] {
                        new Vector2((info.bottom.X+1) / resolution, info.bottom.Y/resolution),
                        new Vector2((info.bottom.X+1) / resolution, (info.bottom.Y+1) / resolution),
                        new Vector2(info.bottom.X/resolution, (info.bottom.Y+1) / resolution),
                        new Vector2(info.bottom.X/resolution, info.bottom.Y/resolution),
                    };

                default:
                    Debug.Warning("Texture Coords do not exist!");
                    return null;
            }
        }

        public static readonly Vector3i[] faceChecks = new Vector3i[6] {
            new Vector3i(0, 0, -1),
            new Vector3i(0, 0, 1),
            new Vector3i(0, 1, 0),
            new Vector3i(0, -1, 0),
            new Vector3i(-1, 0, 0),
            new Vector3i(1, 0, 0)
        };
    }

    public struct TextureInfo {
        public float resolution;
        public string path;

        public TextureInfo(string path, float resolution) {
            this.path = path;
            this.resolution = resolution;
        }
    }
}
