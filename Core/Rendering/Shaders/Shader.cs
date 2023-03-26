using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;

namespace GameEngine.Core.Rendering {
    public class Shader {
        public int ProgramId { get; private set; }

        private ShaderProgramSource _shaderProgramSource { get; }
        private bool isCompiled = false;

        public Shader(ShaderProgramSource shaderProgramSource, bool compile = false) {
            _shaderProgramSource = shaderProgramSource;
            if(compile) {
                Compile();
            }
        }


        public static ShaderProgramSource ParseShader(string path) {
            string[] shaderSource = new string[2];
            EnumShaderType type = EnumShaderType.NONE;
            var allLines = File.ReadAllLines(path);

            for(int i = 0; i <allLines.Length; i++) {
                string current = allLines[i];
                if (current.ToLower().Contains("#shader")) {
                    if (current.ToLower().Contains("vertex")) {
                        type = EnumShaderType.VERTEX;
                    }else if (current.ToLower().Contains("fragment")) {
                        type = EnumShaderType.FRAGMENT;
                    } else {
                        Console.WriteLine("Error. No shader type has been supplied!");
                    }
                } else {
                    shaderSource[(int)type] += current + Environment.NewLine;
                }
            }


            return new ShaderProgramSource(shaderSource[(int)EnumShaderType.VERTEX], shaderSource[(int)EnumShaderType.FRAGMENT]);
        }
    
        public bool Compile() {
            if(_shaderProgramSource == null) {
                Debug.Error("Source is null");
                return false;
            }
            if (isCompiled == true) {
                Debug.Warning("Already Compiled Shader");
                return false;
            }

            int vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderId, _shaderProgramSource.VertexShaderSource);
            GL.CompileShader(vertexShaderId);
            GL.GetShader(vertexShaderId, ShaderParameter.CompileStatus, out var vertexCompilationStatus);
            if (vertexCompilationStatus != (int)All.True) {
                Debug.Warning(GL.GetShaderInfoLog(vertexShaderId));
                return false;
            }

            int fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderId, _shaderProgramSource.FragmentShaderSource);
            GL.CompileShader(fragmentShaderId);
            GL.GetShader(fragmentShaderId, ShaderParameter.CompileStatus, out var fragmentCompilationStatus);
            if (fragmentCompilationStatus != (int)All.True) {
                Debug.Warning(GL.GetShaderInfoLog(fragmentShaderId));
                return false;
            }

            // on the GPU
            ProgramId = GL.CreateProgram();
            GL.AttachShader(ProgramId, vertexShaderId);
            GL.AttachShader(ProgramId, fragmentShaderId);
            GL.LinkProgram(ProgramId);

            // because the program is already cached, dont need the shaders in memory
            GL.DetachShader(ProgramId, vertexShaderId);
            GL.DetachShader(ProgramId, fragmentShaderId);
            GL.DeleteShader(vertexShaderId);
            GL.DeleteShader(fragmentShaderId);
            isCompiled = true;

            return true;
        }

        public void Use() {
            if (isCompiled) {
                GL.UseProgram(ProgramId);
            } else {
                throw new Exception("Shader not compiled! ERROR");
            }
        }
    }
}
