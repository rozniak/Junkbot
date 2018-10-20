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
        /// Initializes this <see cref="GlWorldRenderStrategy"/>.
        /// </summary>
        /// <param name="gameReference">
        /// A reference to the Junkbot game engine.
        /// </param>
        /// <returns>True if the initialization process was successful.</returns>
        public override bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;

            ActorAtlas = GlUtil.LoadAtlas(Environment.CurrentDirectory + @"\Content\Atlas\actors-atlas");

            return true;
        }

        /// <summary>
        /// Renders a portion of the next frame.
        /// </summary>
        public override void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}
