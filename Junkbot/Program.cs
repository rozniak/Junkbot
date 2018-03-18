using Junkbot.Game;
using Junkbot.Game.Input;
using Junkbot.Renderer.Gl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot
{
    class Program
    {
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
