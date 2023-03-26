using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Rendering {
    public class ShaderProgramSource {
        public string VertexShaderSource;
        public string FragmentShaderSource;

        public ShaderProgramSource(string _vertrexSource, string _fragmentSource) {
            VertexShaderSource = _vertrexSource;
            FragmentShaderSource = _fragmentSource;
        }
    }
}
