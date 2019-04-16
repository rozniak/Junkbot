using Pencil.Gaming.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Rzxe.Windowing.Implementations.GlfwFx
{
    internal class GlfwResourceCache : IDisposable
    {
        private Dictionary<string, uint> Atlases { get; set; }

        private bool Disposing { get; set; }

        private Dictionary<string, uint> ShaderPrograms { get; set; }

        
        public GlfwResourceCache()
        {
            Atlases = new Dictionary<string, uint>();
            ShaderPrograms = new Dictionary<string, uint>();
        }


        public uint GetAtlas(string atlasName)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (Disposing)
            {
                throw new ObjectDisposedException("GLFW Texture Cache");
            }

            // Delete all shader programs
            //
            foreach (uint glProgramId in ShaderPrograms.Values)
            {
                GL.DeleteProgram(glProgramId);
            }

            ShaderPrograms.Clear();
            ShaderPrograms = null;
        }

        public uint GetShaderProgram(string programName)
        {
            throw new NotImplementedException();
        }
    }
}
