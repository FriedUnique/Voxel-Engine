

namespace GameEngine.Core.Utilities {
    public interface IBuffer {
        int BufferId { get; }
        void Bind();
        void Unbind();
    }
}