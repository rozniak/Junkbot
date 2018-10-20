using Junkbot.Game;
using Junkbot.Game.Input;
using Junkbot.Renderer.Gl;
using System;
using System.Diagnostics;

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
            var renderer = new GlRenderer();
            var game = new JunkbotGame();
            var gameTimer = new Stopwatch();

            renderer.Start(game);
            game.Begin();
            gameTimer.Start();

            while (renderer.IsOpen)
            {
                TimeSpan deltaTime = gameTimer.Elapsed;
                gameTimer.Reset();

                InputEvents inputs = renderer.GetInputEvents();

                game.Update(deltaTime, inputs);
                renderer.RenderFrame();
            }
        }
    }
}
