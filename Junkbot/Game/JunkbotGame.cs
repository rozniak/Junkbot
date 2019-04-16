using Junkbot.Game.Input;
using Junkbot.Game.State;
using Junkbot.Game.World.Actors.Animation;
using Oddmatics.Rzxe.Game;
using System;
using System.Drawing;

namespace Junkbot.Game
{
    /// <summary>
    /// Represents the main Junkbot game engine.
    /// </summary>
    internal sealed class JunkbotGame : GameEngine
    {
        public override Size DefaultClientWindowSize
        {
            get { return new Size(650, 320); }
        }

        
        public override void Begin()
        {
            base.Begin();

            // Start with the splash screen
            //
            PushState(new SplashGameState());
        }
    }
}
