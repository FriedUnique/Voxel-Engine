using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Core.Utilities.Managers {
    public static class Input {

        public static Vector2 MousePosition => DisplayManager.Instance.window.MousePosition;

        public static Vector2 MouseDelta => DisplayManager.Instance.window.MouseState.Delta;

        public static Vector2 ScrollDelta => DisplayManager.Instance.window.MouseState.ScrollDelta;


        public static bool MousePress(MouseButton button) => DisplayManager.Instance.window.MouseState.IsButtonPressed(button);

        public static bool IsKeyDown(Keys key) => DisplayManager.Instance.window.KeyboardState.IsKeyDown(key);
    }
}
