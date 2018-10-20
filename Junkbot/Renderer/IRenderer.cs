using Junkbot.Game;
using Junkbot.Game.Input;
using System;

namespace Junkbot.Renderer
{
    /// <summary>
    /// Represents a renderer for Junkbot.
    /// </summary>
    internal interface IRenderer : IDisposable
    {
        /// <summary>
        /// Gets the value that indicates whether the renderer window is still open.
        /// </summary>
        bool IsOpen { get; }


        /// <summary>
        /// Retrieves the input events from the renderer window.
        /// </summary>
        /// <returns>
        /// An <see cref="InputEvents"/> instance containing input data pulled from
        /// the renderer window.
        /// </returns>
        InputEvents GetInputEvents();

        /// <summary>
        /// Renders the next frame.
        /// </summary>
        void RenderFrame();

        /// <summary>
        /// Starts this renderer.
        /// </summary>
        /// <param name="gameInstance">
        /// A reference to the Junkbot game engine instance.
        /// </param>
        void Start(JunkbotGame gameInstance);

        /// <summary>
        /// Stops this renderer.
        /// </summary>
        void Stop();
    }
}
