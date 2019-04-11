using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                if (IsRunning)
                {
                    throw new InvalidOperationException(
                        "Cannot set game engine when the game is running."
                        );
                }
                else
                {
                    _GameEngine = value;
                }
            }
        }


        public bool IsRunning { get; private set; }

        private IWindowManager WindowManager { get; set; }


        public void Initialize()
        {
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

        }
    }
}
