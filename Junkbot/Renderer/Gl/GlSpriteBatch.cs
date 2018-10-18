using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;

namespace Junkbot.Renderer.Gl
{
    internal sealed class GlSpriteBatch : IDisposable
    {
        private SpriteAtlas Atlas;

        private int GlCanvasResolutionUniformId;

        private uint GlProgramId;

        private int GlUvMapResolutionUniformId;

        private List<float> VboDrawContents;

        private List<float> VboUvContents;

        private int VertexCount;


        public GlSpriteBatch(string atlasFullPath, uint glProgramId)
        {
            Atlas = GlUtil.LoadAtlas(atlasFullPath);
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


        public void Dispose()
        {
            Atlas.Dispose();
        }

        public void Draw(string spriteName, Rectanglei rect)
        {
            Rectanglei spriteRect = Atlas.GetSpriteUV(spriteName);

            VboDrawContents.AddRange(GlUtil.MakeVboData(rect));
            VboUvContents.AddRange(GlUtil.MakeVboData(spriteRect));

            VertexCount += 12;
        }

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
