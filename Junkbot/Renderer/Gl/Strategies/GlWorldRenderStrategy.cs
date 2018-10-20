using Junkbot.Game;
using Pencil.Gaming.Graphics;
using System;

namespace Junkbot.Renderer.Gl.Strategies
{
    /// <summary>
    /// Renders the game world using the OpenGL API.
    /// </summary>
    internal sealed class GlWorldRenderStrategy : GlRenderStrategy
    {
        /// <summary>
        /// The actor sprite atlas.
        /// </summary>
        private GlSpriteAtlas ActorAtlas;

        /// <summary>
        /// The Junkbot game engine.
        /// </summary>
        private JunkbotGame Game;
        

        /// <summary>
        /// Releases all resources used by this <see cref="GlWorldRenderStrategy"/>.
        /// </summary>
        public override void Dispose()
        {
            ActorAtlas?.Dispose();
        }

        /// <summary>
        /// Initializes this <see cref="GlWorldRenderStrategy"/> with references to the game
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

            ActorAtlas = GlUtil.LoadAtlas(Environment.CurrentDirectory + @"\Content\Atlas\actors-atlas");

            return true;
        }

        /// <summary>
        /// Updates this game state.
        /// </summary>
        /// <param name="deltaTime">The time difference since the last update.</param>
        /// <param name="inputs">The input events that have occurred.</param>
        public override void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}
