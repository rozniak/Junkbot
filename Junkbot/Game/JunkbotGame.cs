using Junkbot.Game.State;
using Junkbot.Game.World.Actors.Animation;
using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;

namespace Junkbot.Game
{
    /// <summary>
    /// Represents the main Junkbot game engine.
    /// </summary>
    internal sealed class JunkbotGame : IGameEngine
    {
        public IGameEngineParameters Parameters
        {
            get { return _Parameters; }
        }
        private IGameEngineParameters _Parameters;

        public GameState CurrentGameState
        {
            get;
            set;
        }


        public JunkbotGame()
        {
            _Parameters = new JunkbotEngineParameters();
        }


        public void Begin()
        {
            CurrentGameState = new SplashGameState();
        }

        public void RenderFrame(IGraphicsController graphics)
        {
            graphics.ClearViewport(Color.CornflowerBlue);

            CurrentGameState.RenderFrame(graphics);
        }

        public void Update(TimeSpan deltaTime, InputEvents inputs)
        {
            CurrentGameState.Update(deltaTime, inputs);
        }
    }
}
