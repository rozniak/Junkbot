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

        private GlResourceCache ResourceCache;


        public GlMenuRenderStrategy(GlResourceCache resourceCache)
        {
            ResourceCache = resourceCache;
        }


        public override void Dispose()
        {
        }

        public override bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;
            
            return true;
        }

        public override void RenderFrame()
        {
            uint simpleUvProgramId = ResourceCache.GetShaderProgram("SimpleUVs");
            var sb = new GlSpriteBatch(
                Environment.CurrentDirectory + @"\Content\Atlas\menu-atlas.png",
                simpleUvProgramId
                );

            sb.Draw(
                "neo_title",
                new Rectanglei(
                    new Vector2i(0, 0),
                    new Vector2i(
                        (int)GlRenderer.JUNKBOT_VIEWPORT.X,
                        (int)GlRenderer.JUNKBOT_VIEWPORT.Y
                        )
                    )
                    );

            sb.Finish();
            sb.Dispose();
        }
    }
}
