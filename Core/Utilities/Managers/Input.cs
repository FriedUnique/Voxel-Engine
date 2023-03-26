using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Core.Utilities.Managers {
    public static class Input {

        public static float MouseDelta(string axis) {
            switch (axis.ToLower()) {
                case "x":
                    return DisplayManager.Instance.window.MouseState.Delta.X;
                case "y":
                    return DisplayManager.Instance.window.MouseState.Delta.Y;
                default:
                    Debug.Warning($"'{axis}' not a valid axis!");
                    return 0.0f;
            }
        }

        public static bool MousePress(MouseButton button) => DisplayManager.Instance.window.MouseState.IsButtonPressed(button);

        public static Vector2 MousePosition() => DisplayManager.Instance.window.MousePosition;

        public static bool IsKeyDown(Keys key) => DisplayManager.Instance.window.KeyboardState.IsKeyDown(key);
    }
}
