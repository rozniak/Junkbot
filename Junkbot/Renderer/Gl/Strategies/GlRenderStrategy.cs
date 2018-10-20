using Junkbot.Game;
using System;

namespace Junkbot.Renderer.Gl.Strategies
{
    /// <summary>
    /// Represents a render strategy that uses the OpenGL API.
    /// </summary>
    internal abstract class GlRenderStrategy : IRenderStrategy
    {
        /// <summary>
        /// Gets or sets the resource cache of OpenGL objects.
        /// </summary>
        public GlResourceCache Resources
        {
            get { return _Resources; }

            set
            {
                if (_Resources != null)
                    throw new InvalidOperationException("GlRenderStrategy: Attempt to set resource cache after one has already been set.");

                _Resources = value;
            }
        }
        private GlResourceCache _Resources;


        /// <summary>
        /// Releases all resources used by this <see cref="GlRenderStrategy"/>.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Initializes this <see cref="GlRenderStrategy"/>.
        /// </summary>
        /// <param name="gameReference">
        /// A reference to the Junkbot game engine.
        /// </param>
        /// <returns>True if the initialization process was successful.</returns>
        public abstract bool Initialize(JunkbotGame gameReference);

        /// <summary>
        /// Renders a portion of the next frame.
        /// </summary>
        public abstract void RenderFrame();
    }
}
