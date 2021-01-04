/**
 * JunkbotEngineParameters.cs - Junkbot Game Engine Parameters
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Game;
using System;
using System.Drawing;
using System.IO;

namespace Junkbot.Game
{
    /// <summary>
    /// Represents parameters defined by the Junkbot game engine.
    /// </summary>
    internal sealed class JunkbotEngineParameters : IGameEngineParameters
    {
        /// <inheritdoc />
        public Size DefaultClientWindowSize
        {
            get { return new Size(650, 420); }
        }
        
        /// <inheritdoc />
        public string GameContentRoot
        {
            get
            {
                return string.Format(
                     "{0}{1}Content",
                     Environment.CurrentDirectory,
                     Path.DirectorySeparatorChar
                );
            }
        }
    }
}
