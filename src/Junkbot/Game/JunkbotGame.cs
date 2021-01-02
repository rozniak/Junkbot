using Junkbot.Game.State;
using Junkbot.Game.World.Level;
using Oddmatics.Rzxe.Game.Actors.Animation;
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
        public AnimationStore Animations { get; private set; }

        public GameState CurrentGameState
        {
            get;
            set;
        }
        
        public JunkbotLevelStore Levels { get; private set; }

        public IGameEngineParameters Parameters { get; private set; }


        public JunkbotGame()
        {
            Parameters = new JunkbotEngineParameters();
        }


        public void Begin()
        {
            Animations = new AnimationStore(Parameters);
            Levels     = new JunkbotLevelStore(this);
            
            CurrentGameState = new SplashGameState(this);
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
