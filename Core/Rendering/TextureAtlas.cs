using GameEngine.Core.Terrain;
using GameEngine.Core.Utilities.Managers;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.IO;

namespace GameEngine.Core.Rendering {

    public class TextureAtlas {
        private TextureObject texture;
        private float resolution;

        public enum BlockType {
            Air,
            Grass,
            Cobble,
            Brick,
            Wood,
            Glass,

            UI_Crosshair
        }
        public enum BlockFace {
            Top,
            Side,
            Bottom
        }

        public TextureAtlas(TextureInfo info) {
            resolution = info.resolution;

            texture = ResourceManager.Instance.LoadTexture(info.path);
            Debug.Log(info.path);
        }

        public void UseTexture() {
            if (texture == null) { return; }
            texture.Use();
        }

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



        public static bool IsBlockTransparent(BlockType type) => TexturePositions[type].isTransparent;
        
        public static Dictionary<BlockType, BlockInfo> TexturePositions = new Dictionary<BlockType, BlockInfo>() {
            { BlockType.Air,            new BlockInfo(new Vector2(7, 7), 1f, true) },

            { BlockType.Grass,          new BlockInfo(new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2), 0f) }, // top, side, bottom
            { BlockType.Cobble,         new BlockInfo(new Vector2(0, 3), 0f) },
            { BlockType.Brick,          new BlockInfo(new Vector2(0, 4), 0f) },
            { BlockType.Wood,           new BlockInfo(new Vector2(0, 6), new Vector2(0, 5), new Vector2(0, 6), 0f) },
            { BlockType.Glass,          new BlockInfo(new Vector2(0, 7), 0.8f, true) },

            { BlockType.UI_Crosshair,   new BlockInfo(new Vector2(1, 0), 1f, true) }
        };


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

        /// <summary>
        /// Stores information about the Texture
        /// <paramref name="resolution"/> Tells how many textures there are per side (128x128 atlas and 16x16 textures -> 8)
        /// </summary>
        public TextureInfo(string path, float resolution) {
            this.path = path;
            this.resolution = resolution;
        }
    }
}
