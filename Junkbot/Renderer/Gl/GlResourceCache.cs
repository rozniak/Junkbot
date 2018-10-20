using Pencil.Gaming.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Junkbot.Renderer.Gl
{
    /// <summary>
    /// Stores OpenGL objects so they may be created and reused by multiple render
    /// strategies.
    /// </summary>
    internal sealed class GlResourceCache : IDisposable
    {
        /// <summary>
        /// Gets the value that indicates whether this <see cref="GlResourceCache"/>
        /// instance has been disposed.
        /// </summary>
        public bool Disposed { get; private set; }


        /// <summary>
        /// The value that indicates whether this <see cref="GlResourceCache"/> is
        /// being disposed.
        /// </summary>
        private bool Disposing;

        /// <summary>
        /// The mapping of names to cached OpenGL Shader Programs.
        /// </summary>
        private Dictionary<string, uint> ShaderPrograms;


        /// <summary>
        /// Initializes a new instance of the <see cref="GlResourceCache"/> class.
        /// </summary>
        public GlResourceCache()
        {
            Disposing = false;
            ShaderPrograms = new Dictionary<string, uint>();
        }


        /// <summary>
        /// Releases all resources used by this <see cref="GlResourceCache"/>.
        /// </summary>
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

        /// <summary>
        /// Gets an OpenGL Shader Program from the resource cache - if the program is
        /// not already cached, an attempt will be made to load it.
        /// </summary>
        /// <param name="programName">The name of the program.</param>
        /// <returns>The ID of the OpenGL Shader Program object.</returns>
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
