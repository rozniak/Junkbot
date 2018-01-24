using Junkbot.Game;
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

            gameTimer.Start();

            while (renderer.IsOpen)
            {
                TimeSpan deltaTime = gameTimer.Elapsed;
                gameTimer.Reset();

                game.Update(deltaTime);
                renderer.RenderFrame();
            }

            Console.ReadKey(true);
        }
    }
}
