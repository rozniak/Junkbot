using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Windowing;
using Oddmatics.Rzxe.Windowing.Implementations.GlfwFx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Oddmatics.Rzxe
{
    public sealed class GameEntryPoint
    {
        private IGameEngine _GameEngine;
        public IGameEngine GameEngine
        {
            get { return _GameEngine; }
            set
            {
                if (Locked)
                {
                    throw new InvalidOperationException(
                        "Game engine state has been locked."
                        );
                }
                else
                {
                    _GameEngine = value;
                }
            }
        }


        public bool Locked { get; private set; }

        private IWindowManager WindowManager { get; set; }


        public void Initialize()
        {
            if (Locked)
            {
                throw new InvalidOperationException(
                    "Game engine state has been locked."
                    );
            }

            if (GameEngine == null)
            {
                throw new InvalidOperationException(
                    "No game engine provided."
                    );
            }

            // FIXME: Replace this one day with a way of selecting the window manager
            //
            WindowManager = new GlfwWindowManager()
            {
                RenderedGameEngine = GameEngine
            };
            
            WindowManager.Initialize();
        }

        public void Run()
        {
            if (WindowManager == null || !WindowManager.Ready)
            {
                throw new InvalidOperationException("No window manager initialized.");
            }

            // Enter the main game loop
            //
            var gameTime = new Stopwatch();

            gameTime.Start();
            GameEngine.Begin();

            while (WindowManager.IsOpen)
            {
                TimeSpan deltaTime = gameTime.Elapsed;
                gameTime.Reset();

                InputEvents inputs = WindowManager.ReadInputEvents();
                GameEngine.Update(deltaTime, inputs);

                WindowManager.RenderFrame();

                Thread.Yield();
            }
        }
    }
}
