using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System.Drawing;

namespace GameEngine.Core.Utilities.Managers {
    public class DisplayManager {
        private static DisplayManager _instance = null;
        private static readonly object _lock = new();
        public GameWindow window;

        public static DisplayManager Instance {
            get {
                lock (_lock) {
                    if (_instance is null) {
                        _instance = new DisplayManager();
                    }
                    return _instance;
                }
            }
        }

        public unsafe GameWindow CreateWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) {
            window = new GameWindow(gameWindowSettings, nativeWindowSettings);
            MonitorInfo currentMonitor = Monitors.GetMonitorFromWindow(window.WindowPtr);
            Rectangle monitorRectangle = new Rectangle(0, 0, currentMonitor.ClientArea.Size.X, currentMonitor.ClientArea.Size.Y);

            int x = (monitorRectangle.Right + monitorRectangle.Left - nativeWindowSettings.Size.X) / 2;
            int y = (monitorRectangle.Bottom + monitorRectangle.Top - nativeWindowSettings.Size.Y) / 2;
            if (x < monitorRectangle.Left) {
                x = monitorRectangle.Left;
            }
            if (y < monitorRectangle.Top) {
                y = monitorRectangle.Top;
            }
            window.ClientRectangle = new Box2i(x, y, x + nativeWindowSettings.Size.X, y + nativeWindowSettings.Size.Y);
            return window;
        }
    }
}