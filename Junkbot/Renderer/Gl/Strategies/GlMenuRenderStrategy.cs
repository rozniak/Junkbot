using Junkbot.Game;
using Pencil.Gaming.MathUtils;
using System;

namespace Junkbot.Renderer.Gl.Strategies
{
    /// <summary>
    /// Renders the main menu chrome using the OpenGL API.
    /// </summary>
    internal sealed class GlMenuRenderStrategy : GlRenderStrategy
    {
        /// <summary>
        /// The Junkbot game engine.
        /// </summary>
        private JunkbotGame Game;

        /// <summary>
        /// The OpenGL resource cache.
        /// </summary>
        private GlResourceCache ResourceCache;


        /// <summary>
        /// Initializes a new instance of the <see cref="GlMenuRenderStrategy"/> class.
        /// </summary>
        /// <param name="resourceCache">
        /// A reference to the OpenGL resource cache.
        /// </param>
        public GlMenuRenderStrategy(GlResourceCache resourceCache)
        {
            ResourceCache = resourceCache;
        }


        /// <summary>
        /// Releases all resources used by this <see cref="GlMenuRenderStrategy"/>.
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// Initializes this <see cref="GlMenuRenderStrategy"/>.
        /// </summary>
        /// <param name="gameReference">
        /// A reference to the Junkbot game engine.
        /// </param>
        /// <returns>True if the initialization process was successful.</returns>
        public override bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;
            
            return true;
        }

        /// <summary>
        /// Renders a portion of the next frame.
        /// </summary>
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
