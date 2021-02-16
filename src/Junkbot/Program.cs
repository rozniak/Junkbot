/**
 * Program.cs - Junkbot Entry Point
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game;
using Oddmatics.Rzxe.Game.Hosting;

namespace Junkbot
{
    /// <summary>
    /// The main program class for Junkbot.
    /// </summary>
    internal sealed class Program
    {
        /// <summary>
        /// The main entry point for Junkbot.
        /// </summary>
        static void Main(
            string[] args
        )
        {
            var host = new EngineHost(new JunkbotGame());

            host.Initialize();
            host.Run();
        }
    }
}
