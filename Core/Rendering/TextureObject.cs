using System;
using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Core.Rendering {
    public class TextureObject : IDisposable {
        private bool _disposed;
        public int Handle { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public TextureUnit TextureSlot { get; set; } = TextureUnit.Texture0;

        public TextureObject(int handle) {
            Handle = handle;
        }

        public TextureObject(int handle, int width, int height) : this(handle) {
            Width = width;
            Height = height;
        }

        public TextureObject(int handle, int width, int height, TextureUnit textureSlot) : this(handle, width, height) {
            TextureSlot = textureSlot;
        }

        ~TextureObject() {
            Dispose(false);
        }

        public void Use() {
            GL.ActiveTexture(TextureSlot);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Dispose(bool disposing) {
            if (!_disposed) {
                GL.DeleteTexture(Handle);
                _disposed = true;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}