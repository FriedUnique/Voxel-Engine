using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Core.Utilities {
    public class VertexBuffer : IBuffer {
        public int BufferId { get; }

        public VertexBuffer() {
            BufferId = GL.GenBuffer();
        }

        public VertexBuffer(float[] vertices, BufferUsageHint hint = BufferUsageHint.StaticDraw) {
            BufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, BufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, hint);

        }

        public void LoadData(float[] vertices, BufferUsageHint hint = BufferUsageHint.StaticDraw) {
            GL.BindBuffer(BufferTarget.ArrayBuffer, BufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, hint);
        }

        public void Bind() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, BufferId);
        }

        public void Unbind() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}