using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Core.Utilities {
    public static class Utilities {
        public static int GetSizeOfVertexAttribPointerType(VertexAttribPointerType attribType) {
            switch (attribType) {
                case VertexAttribPointerType.UnsignedByte:
                    return 1;
                case VertexAttribPointerType.UnsignedInt:
                    return 4;
                case VertexAttribPointerType.Float:
                    return 4;
                default:
                    return 0;
            }
        }
    }
}