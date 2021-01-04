/**
 * JunkbotGame.cs - Junkbot Game Engine
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

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
    /// Represents the Junkbot game engine.
    /// </summary>
    internal sealed class JunkbotGame : IGameEngine
    {
        /// <summary>
        /// Gets the animation store.
        /// </summary>
        public AnimationStore Animations { get; private set; }
        
        /// <inheritdoc />
        public GameState CurrentGameState { get; set; }
        
        /// <summary>
        /// Gets the level store.
        /// </summary>
        public JunkbotLevelStore Levels { get; private set; }
        
        /// <inheritdoc />
        public IGameEngineParameters Parameters { get; private set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotGame"/>.
        /// </summary>
        public JunkbotGame()
        {
            Parameters = new JunkbotEngineParameters();
        }
        
        
        /// <inheritdoc />
        public void Begin()
        {
            Animations = new AnimationStore(Parameters);
            Levels     = new JunkbotLevelStore(this);
            
            CurrentGameState = new SplashGameState(this);
        }
        
        /// <inheritdoc />
        public void RenderFrame(
            IGraphicsController graphics
        )
        {
            graphics.ClearViewport(Color.CornflowerBlue);

            CurrentGameState.RenderFrame(graphics);
        }
        
        /// <inheritdoc />
        public void Update(
            TimeSpan    deltaTime,
            InputEvents inputs
        )
        {
            CurrentGameState.Update(deltaTime, inputs);
        }
    }
}
