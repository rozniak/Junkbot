using Pencil.Gaming.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer.Gl
{
    internal class GlResourceCache : IDisposable
    {
        public bool Disposed { get; private set; }


        private bool Disposing;
        private Dictionary<string, uint> ShaderPrograms;


        public GlResourceCache()
        {
            Disposing = false;
            ShaderPrograms = new Dictionary<string, uint>();
        }


        public void Dispose()
        {
            Disposing = true;

            foreach (uint glProgramId in ShaderPrograms.Values)
            {
                GL.DeleteProgram(glProgramId);
            }

            ShaderPrograms.Clear();
            ShaderPrograms = null;

            Disposed = true;
            Disposing = false;
        }

        public uint GetShaderProgram(string programName)
        {
            if (Disposing || Disposed)
                return 0;

            // If the shader program has previously been loaded, return its GL
            // ID
            //
            if (ShaderPrograms.ContainsKey(programName))
                return ShaderPrograms[programName];

            // We reached here, program has not yet been cached, need to 
            // compile it from sources
            //
            string fragmentSource = File.ReadAllText(Environment.CurrentDirectory + @"\Content\Shaders\OpenGL\" + programName + @"\fragment.glsl");
            string vertexSource = File.ReadAllText(Environment.CurrentDirectory + @"\Content\Shaders\OpenGL\" + programName + @"\vertex.glsl");

            uint compiledProgramId = GlUtil.CompileShaderProgram(vertexSource, fragmentSource);

            ShaderPrograms.Add(programName, compiledProgramId);
            
            return compiledProgramId;
        }
    }
}
