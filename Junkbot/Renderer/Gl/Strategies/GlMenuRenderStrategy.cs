using Junkbot.Game;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer.Gl.Strategies
{
    internal class GlMenuRenderStrategy : GlRenderStrategy
    {
        private JunkbotGame Game;

        private SpriteAtlas MenuAtlas;


        #region SimpleUV Shader Program

        private uint SimpleUVProgramId;
        private int SimpleUVCanvasResUniformId;
        private int SimpleUVMapResUniformId;

        #endregion


        private int TitleScreenDrawVboId;
        private int TitleScreenUVVboId;


        public override void Dispose()
        {
            MenuAtlas?.Dispose();
        }

        public override bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;
            MenuAtlas = GlUtil.LoadAtlas(Environment.CurrentDirectory + @"\Content\Atlas\menu-atlas.png");

            // Initialize VAO
            //
            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            // Retrieve shader program properties
            //
            SimpleUVProgramId = Resources.GetShaderProgram("SimpleUVs");

            SimpleUVCanvasResUniformId = GL.GetUniformLocation(
                SimpleUVProgramId,
                "CanvasResolution"
                );
            SimpleUVMapResUniformId = GL.GetUniformLocation(
                SimpleUVProgramId, 
                "UvMapResolution"
                );

            // Create VBOs
            //
            Rectanglei neoTitleRect = MenuAtlas.GetSpriteUV("neo_title");

            TitleScreenDrawVboId = GlUtil.MakeVbo(
                new Rectanglei(Vector2i.Zero, neoTitleRect.Size),
                BufferUsageHint.StaticDraw
                );
            TitleScreenUVVboId = GlUtil.MakeVbo(
                neoTitleRect,
                BufferUsageHint.StaticDraw
                );

            return true;
        }

        public override void RenderFrame()
        {
            // Set up shader program
            //
            GL.UseProgram(SimpleUVProgramId);

            GL.Uniform2(SimpleUVCanvasResUniformId, GlRenderer.JUNKBOT_VIEWPORT);
            GL.Uniform2(SimpleUVMapResUniformId, MenuAtlas.Size);
            
            // Bind the menu atlas
            //
            GL.BindTexture(TextureTarget.Texture2D, MenuAtlas.GlTextureId);

            // Render now
            //
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, TitleScreenDrawVboId);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, TitleScreenUVVboId);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawArrays(BeginMode.Triangles, 0, 12);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
        }
    }
}
