using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using GameEngine.Core.Rendering;
using GameEngine.Core.Utilities.Managers;

namespace GameEngine.Core
{
    public abstract class Game {
        public static Vector2 windowSize;

        protected string WindowTitle { get; set; }
        protected int InitialWindowWidth { get; set; }
        protected int InitialWindowHeight { get; set; }

        private GameWindowSettings _gameWindowSettings = GameWindowSettings.Default;
        private NativeWindowSettings _nativeWindowSettings = NativeWindowSettings.Default;

        public Game(string windowTitle, int initialWindowWidth, int initialWindowHeight) {
            WindowTitle = windowTitle;
            InitialWindowWidth = initialWindowWidth;
            InitialWindowHeight = initialWindowHeight;
            _nativeWindowSettings.Size = new Vector2i(initialWindowWidth, initialWindowHeight);
            _nativeWindowSettings.Title = windowTitle;

            windowSize = new Vector2(InitialWindowWidth, InitialWindowHeight);
        }

        public void Run() {
            Init();

            using GameWindow gameWindow = DisplayManager.Instance.CreateWindow(_gameWindowSettings, _nativeWindowSettings);
            GameTime gameTime = new GameTime();
            gameTime.NormalTick();

            Camera.aspectRatio = (float)InitialWindowWidth / InitialWindowHeight;

            // settings of window etc
            gameWindow.VSync = VSyncMode.On; // less workload for the CPU;
            // confines the mouse to the window
            gameWindow.CursorState = CursorState.Grabbed;

            gameWindow.Load += () => { 
                OnLoad();
            };
            gameWindow.UpdateFrame += (FrameEventArgs args) => {
                gameTime.NormalTick();
                Update(gameTime);
            };

            gameWindow.RenderFrame += (FrameEventArgs args) => {
                Render(gameTime);
                gameWindow.SwapBuffers();
            };

            gameWindow.Resize += (ResizeEventArgs args) => {
                GL.Viewport(0, 0, gameWindow.Size.X, gameWindow.Size.Y);
                Camera.aspectRatio = (float)gameWindow.Size.X / gameWindow.Size.Y;
                windowSize = new Vector2(InitialWindowWidth, InitialWindowHeight);
            };


            gameWindow.MouseDown += (MouseButtonEventArgs args) => {
                gameWindow.CursorState = CursorState.Grabbed;
            };

            gameWindow.KeyDown += (KeyboardKeyEventArgs args) => {
                if (args.Key == Keys.Escape) {
                    gameWindow.CursorState = CursorState.Normal;
                }
            };

            gameWindow.Run();
        }

        protected abstract void Init();
        protected abstract void OnLoad();
        protected abstract void Update(GameTime time);
        protected abstract void Render(GameTime time);
    }
}
