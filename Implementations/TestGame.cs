using GameEngine.Core.Rendering;
using GameEngine.Core.Terrain;
using GameEngine.Core.Utilities.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GameEngine.Core {

    internal class TestGame : Game {
        public TestGame(string windowTitle, int initialWindowWidth, int initialWindowHeight) : base(windowTitle, initialWindowWidth, initialWindowHeight) { }

        // the textures and shaders inside the resources folder have to be set to copy always
        private const string texturePath = "Resources/Textures/AtlasTexture.png";
        private const string shaderPath = "Resources/Shaders/Default.shader";

        private Shader shader;
        private TerrainGenerator tg;
        private Player player;

        protected override void Init() {
            Debug.level = 0;
        }

        protected override void OnLoad() {
            shader = new Shader(Shader.ParseShader(shaderPath));
            shader.Compile();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            TextureInfo info = new TextureInfo(texturePath, 16f); // specific for this texture atlas

            player = new Player(new Vector3(14, 20, 12));
            tg = new TerrainGenerator(info, 3, player);
        }

        protected override void Render(GameTime time) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color4.CornflowerBlue);

            tg.UseTexture();
            shader.Use();

            tg.BindAll();

            Matrix4 mvp = Matrix4.CreateRotationY(0) * player.cam.GetViewMatrix() * player.cam.GetProjectionMatrix();
            int mvpID = GL.GetUniformLocation(shader.ProgramId, "MVP");
            GL.UniformMatrix4(mvpID, false, ref mvp);

        }

        protected override void Update(GameTime time) {
            player.Update(time.deltaTime);
        }
    }
}



#region UnUsedCode
/*
Matrix4 model = Matrix4.Identity;
Matrix4 view = Matrix4.Identity;
Matrix4 proj = Matrix4.Identity;

Matrix4.CreateRotationY(r, out model);
model = Matrix4.LookAt(new Vector3(-6, -20, -15), new Vector3(-6, -20, -25) - Vector3.UnitZ, Vector3.UnitY);
Matrix4.CreateTranslation(new Vector3(-6, -20, -24), out view);
Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)InitialWindowWidth / InitialWindowHeight, 0.1f, 100000f, out proj); 

Matrix4 model = Matrix4.Identity;
Matrix4 view = Matrix4.Identity;
Matrix4 proj = Matrix4.Identity;

Matrix4.CreateRotationY(r, out model);
Matrix4.CreateTranslation(new Vector3(-6, -20, -25), out view);
Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float) InitialWindowWidth / InitialWindowHeight, 0.1f, 100000f, out proj);

int modelID = GL.GetUniformLocation(shader.ProgramId, "model");
int viewID = GL.GetUniformLocation(shader.ProgramId, "view");
int projID = GL.GetUniformLocation(shader.ProgramId, "proj");

GL.UniformMatrix4(modelID, false, ref model);
GL.UniformMatrix4(viewID, false, ref view);
GL.UniformMatrix4(projID, false, ref proj);
 
 
*/

#endregion
