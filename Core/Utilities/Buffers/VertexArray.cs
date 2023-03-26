using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Core.Utilities {
    public class VertexArray : IBuffer {
        public int BufferId { get; }

        public VertexArray() {
            BufferId = GL.GenVertexArray();
        }


        /// <summary>
        /// I dont know what the fuck is happening here but when deleting vertex array, a system access violation is thrown at me
        /// </summary>
        ~VertexArray() {
            //GL.DeleteVertexArray(BufferId);
        }

        public void AddBuffer(VertexBuffer vertexBuffer, BufferLayout bufferLayout) {
            Bind();
            vertexBuffer.Bind();
            List<BufferElement> elements = bufferLayout.GetBufferElements();

            int offset = 0;
            for (int i = 0; i < elements.Count(); i++) {
                BufferElement currentElement = elements[i];
                GL.EnableVertexAttribArray(i);

                // set the attributes 
                GL.VertexAttribPointer(i, currentElement.Count, currentElement.Type, currentElement.Normalized, bufferLayout.GetStride(), offset);
                offset += currentElement.Count * Utilities.GetSizeOfVertexAttribPointerType(currentElement.Type);
            }
        }

        public void RemoveBuffer(VertexBuffer vertexBuffer) {
            vertexBuffer.Unbind();
            Unbind();
        }


        public void Bind() {
            GL.BindVertexArray(BufferId);
        }

        public void Unbind() {
            GL.BindVertexArray(0);
        }
    }
}