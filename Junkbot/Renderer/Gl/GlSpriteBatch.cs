using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;

namespace Junkbot.Renderer.Gl
{
    /// <summary>
    /// Processes a stream of draw commands as a batch to supply to OpenGL.
    /// </summary>
    internal sealed class GlSpriteBatch
    {
        /// <summary>
        /// The sprite atlas to use during this batch.
        /// </summary>
        private GlSpriteAtlas Atlas;

        /// <summary>
        /// The ID of the CanvasResolution uniform variable in the associated OpenGL
        /// Shader Program.
        /// </summary>
        private int GlCanvasResolutionUniformId;

        /// <summary>
        /// The ID of the OpenGL Shader Program to use.
        /// </summary>
        private uint GlProgramId;

        /// <summary>
        /// The ID of the "UvMapResolution" uniform variable in the associated OpenGL
        /// Shader Program.
        /// </summary>
        private int GlUvMapResolutionUniformId;

        /// <summary>
        /// The vertex data for drawing the sprites.
        /// </summary>
        private List<float> VboDrawContents;

        /// <summary>
        /// The vertex data to use for blitting sprite texture data.
        /// </summary>
        private List<float> VboUvContents;

        /// <summary>
        /// The number of vertices to draw in this batch.
        /// </summary>
        private int VertexCount;


        /// <summary>
        /// Initializes a new instance of the <see cref="GlSpriteBatch"/> class.
        /// </summary>
        /// <param name="atlasFullPath">
        /// The atlas to use when drawing sprites.
        /// </param>
        /// <param name="glProgramId">
        /// The ID of the OpenGL Shader Program to use.
        /// </param>
        public GlSpriteBatch(GlSpriteAtlas atlas, uint glProgramId)
        {
            Atlas = atlas;
            GlProgramId = glProgramId;

            GlCanvasResolutionUniformId = GL.GetUniformLocation(
                GlProgramId,
                "CanvasResolution"
                );

            GlUvMapResolutionUniformId = GL.GetUniformLocation(
                GlProgramId,
                "UvMapResolution"
                );

            VertexCount = 0;
            VboDrawContents = new List<float>();
            VboUvContents = new List<float>();
        }
        

        /// <summary>
        /// Issues a draw command to this batch.
        /// </summary>
        /// <param name="spriteName">The name of the sprite.</param>
        /// <param name="rect">The rectangle space to draw the sprite at.</param>
        public void Draw(string spriteName, Rectanglei rect)
        {
            Rectanglei spriteRect = Atlas.GetSpriteUV(spriteName);

            VboDrawContents.AddRange(GlUtil.MakeVboData(rect));
            VboUvContents.AddRange(GlUtil.MakeVboData(spriteRect));

            VertexCount += 12;
        }

        /// <summary>
        /// Finishes this batch and submits it to OpenGL for drawing.
        /// </summary>
        public void Finish()
        {
            // Create VBO for the batch
            //
            int vboDrawId = GL.GenBuffer();
            int vboUvId = GL.GenBuffer();
            float[] spriteDrawVerts = VboDrawContents.ToArray();
            float[] spriteUvVerts = VboUvContents.ToArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboDrawId);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                new IntPtr(sizeof(float) * spriteDrawVerts.Length),
                spriteDrawVerts,
                BufferUsageHint.StreamDraw
                );

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboUvId);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                new IntPtr(sizeof(float) * spriteUvVerts.Length),
                spriteUvVerts,
                BufferUsageHint.StreamDraw
                );

            // Set up shader program
            //
            GL.UseProgram(GlProgramId);

            GL.Uniform2(GlCanvasResolutionUniformId, GlRenderer.JUNKBOT_VIEWPORT);
            GL.Uniform2(GlUvMapResolutionUniformId, Atlas.Size);

            // Bind the atlas
            //
            GL.BindTexture(TextureTarget.Texture2D, Atlas.GlTextureId);

            // Assign draw attrib
            //
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboDrawId);
            GL.VertexAttribPointer(
                0,
                2,
                VertexAttribPointerType.Float,
                false,
                0,
                0
                );

            // Assign UV attrib
            //
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboUvId);
            GL.VertexAttribPointer(
                1,
                2,
                VertexAttribPointerType.Float,
                false,
                0,
                0
                );


            // Draw now!
            //
            GL.DrawArrays(BeginMode.Triangles, 0, VertexCount);
            
            // Detach and destroy VBOs
            //
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);

            GL.DeleteBuffer(vboDrawId);
            GL.DeleteBuffer(vboUvId);
        }
    }
}
