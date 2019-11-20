using Junkbot.Game;
using Junkbot.Game.Input;
using Oddmatics.Rzxe;
using System;
using System.Diagnostics;
using System.Threading;

namespace Junkbot
{
    /// <summary>
    /// The main program class.
    /// </summary>
    internal sealed class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var entryPoint = new GameEntryPoint()
            {
                GameEngine = new JunkbotGame()
            };

            entryPoint.Initialize();
            entryPoint.Run();
        }
    }
}
