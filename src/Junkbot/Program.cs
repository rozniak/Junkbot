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
            // Set up game
            //
            // TODO: Support booting different games via CMD arg - for now just load
            //       up Junkbot
            //
            var game = new JunkbotGame();

            game.SelectGame("Junkbot");
            
            // Boot into engine now
            //
            var host = new EngineHost(game);

            host.Initialize();
            host.Run();
        }
    }
}
