using Junkbot.Game;
using System;

namespace Junkbot.Renderer
{
    /// <summary>
    /// Represents a stategy for rendering a section of Junkbot.
    /// </summary>
    internal interface IRenderStrategy : IDisposable
    {
        /// <summary>
        /// Initializes this <see cref="IRenderStrategy"/>.
        /// </summary>
        /// <param name="gameReference">
        /// A reference to the Junkbot game engine.
        /// </param>
        /// <returns>True if the initialization process was successful.</returns>
        bool Initialize(JunkbotGame gameReference);

        /// <summary>
        /// Renders a portion of the next frame.
        /// </summary>
        void RenderFrame();
    }
}
