using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Core.Utilities {
    public class IndexBuffer : IBuffer {
        public int BufferId { get; }

        public IndexBuffer() {
            BufferId = GL.GenBuffer();
        }

        public IndexBuffer(uint[] indices) {
            BufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, BufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.DynamicDraw);
        }

        public void LoadData(uint[] indices, BufferUsageHint hint = BufferUsageHint.DynamicDraw) {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, BufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, hint);
        }

        public void Bind() {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, BufferId);
        }

        public void Unbind() {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
    }
}