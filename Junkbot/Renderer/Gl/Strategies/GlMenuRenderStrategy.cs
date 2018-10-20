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
        /// Initializes this <see cref="GlMenuRenderStrategy"/> with references to the game
        /// engine and animation store repository instances.
        /// </summary>
        /// <param name="gameReference">
        /// A reference to the game engine instance.
        /// </param>
        /// <param name="animationStore">
        /// A reference to the animation store repository instance.
        /// </param>
        /// <returns>True if the initialization routine was successful.</returns>
        public override bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;
            
            return true;
        }

        /// <summary>
        /// Updates this game state.
        /// </summary>
        /// <param name="deltaTime">The time difference since the last update.</param>
        /// <param name="inputs">The input events that have occurred.</param>
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
