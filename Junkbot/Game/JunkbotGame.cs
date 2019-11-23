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
        public override IGameEngineParameters Parameters
        {
            get { return _Parameters; }
        }
        private IGameEngineParameters _Parameters;


        public JunkbotGame()
        {
            _Parameters = new JunkbotEngineParameters();
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
