using Newtonsoft.Json.Linq;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Junkbot.Renderer.Gl
{
    /// <summary>
    /// Provides various helper methods for doing common tasks with the OpenGL API.
    /// </summary>
    internal static class GlUtil
    {
        /// <summary>
        /// Compiles GLSL source code to an OpenGL Shader Program.
        /// </summary>
        /// <param name="vertexSource">The vertex shader GLSL source code.</param>
        /// <param name="fragmentSource">The fragment shader GLSL source code.</param>
        /// <returns>The ID of the compiled OpenGL Shader Program.</returns>
        public static uint CompileShaderProgram(string vertexSource, string fragmentSource)
        {
            string infoLog = String.Empty; // Store any error information

            // Create the shader objects
            //
            uint vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            uint fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);

            // Compile vertex shader
            //
            GL.ShaderSource(vertexShaderId, vertexSource);
            GL.CompileShader(vertexShaderId);

            GL.GetShaderInfoLog((int)vertexShaderId, out infoLog);

            if (infoLog.Length > 0)
                throw new ArgumentException("GlUtil: Failed to compile vertex shader, message: " + infoLog);

            // Compile fragment shader
            //
            GL.ShaderSource(fragmentShaderId, fragmentSource);
            GL.CompileShader(fragmentShaderId);

            GL.GetShaderInfoLog((int)fragmentShaderId, out infoLog);

            if (infoLog.Length > 0)
                throw new ArgumentException("GlUtil: Failed to compile fragment shader, message: " + infoLog);

            // Link the program
            //
            uint programId = GL.CreateProgram();

            GL.AttachShader(programId, vertexShaderId);
            GL.AttachShader(programId, fragmentShaderId);
            GL.LinkProgram(programId);

            GL.GetProgramInfoLog((int)programId, out infoLog);

            if (infoLog.Length > 0)
                throw new ArgumentException("GlUtil: Failed to link shaders to program, message: " + infoLog);

            // Delete old compiled shader source objects
            //
            GL.DetachShader(programId, vertexShaderId);
            GL.DetachShader(programId, fragmentShaderId);
            GL.DeleteShader(vertexShaderId);
            GL.DeleteShader(fragmentShaderId);

            return programId;
        }

        /// <summary>
        /// Loads a sprite atlas from supplied files on disk.
        /// </summary>
        /// <param name="filename">
        /// The filename of the atlas, the extension will be ignored.
        /// </param>
        /// <returns>
        /// A <see cref="GlSpriteAtlas"/> created from the supplied data.
        /// </returns>
        public static GlSpriteAtlas LoadAtlas(string filename)
        {
            // Read texture atlas information and bitmap data
            //
            string atlasPath = Path.GetDirectoryName(filename);
            string atlasNoExt = Path.GetFileNameWithoutExtension(filename);

            var atlasBmp = (Bitmap)Image.FromFile(atlasPath + @"\" + atlasNoExt + ".png");
            var atlasJson = File.ReadAllText(atlasPath + @"\" + atlasNoExt + ".json");
            var atlasNodeArray = JArray.Parse(atlasJson);

            var atlasMap = new Dictionary<string, Rectanglei>();
            
            foreach (JToken token in atlasNodeArray)
            {
                string key = token.Value<string>("Name").ToLower();
                string boundsCsv = token.Value<string>("Bounds");
                var rectangleComponents = new List<int>();

                foreach (string boundComponent in boundsCsv.Split(','))
                {
                    rectangleComponents.Add(Convert.ToInt32(boundComponent));
                }

                atlasMap.Add(
                    key,
                    new Rectanglei(
                        rectangleComponents[0],
                        rectangleComponents[1],
                        rectangleComponents[2],
                        rectangleComponents[3]
                        )
                    );
            }

            // Read out atlas dimensions
            //
            Vector2 atlasDimensions = new Vector2(
                atlasBmp.Width,
                atlasBmp.Height
                );

            // Load the bitmap into OpenGL
            //
            int glTextureId = GlUtil.LoadBitmapTexture(atlasBmp);

            // Dispose the atlas resource
            //
            atlasBmp.Dispose();

            return new GlSpriteAtlas(atlasDimensions, glTextureId, atlasMap);
        }

        /// <summary>
        /// Loads the specified <see cref="Bitmap"/> into an OpenGL texture object.
        /// </summary>
        /// <param name="bmp">The bitmap image.</param>
        /// <returns>The ID of the new OpenGL texture object.</returns>
        public static int LoadBitmapTexture(Bitmap bmp)
        {
            // Flip texture before processing
            //
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            // Acquire bitmap raw data handle
            //
            BitmapData data = bmp.LockBits(
                new System.Drawing.Rectangle(Point.Empty, bmp.Size),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );

            // Create the OpenGL texture
            //
            int textureId = GL.GenTexture();

            // Bind texture and load bitmap data
            //
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba8,
                bmp.Width,
                bmp.Height,
                0,
                Pencil.Gaming.Graphics.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0
                );

            // Set texture parameters
            //
            GL.TexParameterI(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                new int[] { (int)TextureMagFilter.Linear }
                );
            GL.TexParameterI(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                new int[] { (int)TextureMinFilter.Linear }
                );

            // Unlock bitmap raw data
            //
            bmp.UnlockBits(data);

            return textureId;
        }

        /// <summary>
        /// Creates data from the specified <see cref="Rectanglei"/> that can be
        /// uploaded into a vertex buffer object.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>
        /// A float array that contains vertex data for the <see cref="Rectanglei"/>.
        /// </returns>
        public static float[] MakeVboData(Rectanglei rect)
        {
            // Expand the rectangle to coordinates
            //
            float[] rectPoints = new float[]
            {
                rect.Left, rect.Bottom,
                rect.Left, rect.Top,
                rect.Right, rect.Top,

                rect.Left, rect.Bottom,
                rect.Right, rect.Top,
                rect.Right, rect.Bottom
            };

            return rectPoints;
        }
    }
}
