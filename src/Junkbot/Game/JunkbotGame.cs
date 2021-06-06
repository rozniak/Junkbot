/**
 * JunkbotGame.cs - Junkbot Game Engine
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.Profile;
using Junkbot.Game.State;
using Junkbot.Game.World.Level;
using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Game.Animation;
using Oddmatics.Rzxe.Game.Hosting;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;

namespace Junkbot.Game
{
    /// <summary>
    /// Represents the Junkbot game engine.
    /// </summary>
    public sealed class JunkbotGame : IGameEngine
    {
        /// <summary>
        /// Gets the animation store.
        /// </summary>
        public SpriteAnimationStore Animations { get; private set; }
        
        /// <inheritdoc />
        public GameState CurrentGameState { get; set; }
        
        /// <summary>
        /// Gets the engine host.
        /// </summary>
        public IEngineHost EngineHost { get; private set; }
        
        /// <summary>
        /// Gets the value that indicates whether a game is currently loaded.
        /// </summary>
        public bool GameLoaded
        {
            get { return GameName != null; }
        }

        /// <summary>
        /// Gets the name of the Junkbot game being played.
        /// </summary>
        public string GameName { get; private set; }

        /// <summary>
        /// Gets the level store.
        /// </summary>
        public JunkbotLevelStore Levels { get; private set; }
        
        /// <inheritdoc />
        public IGameEngineParameters Parameters { get; private set; }
        
        /// <summary>
        /// Gets the value that indicates whether the game is running.
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        /// Gets the user's profile data.
        /// </summary>
        public JunkbotProfile UserProfile { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotGame"/>.
        /// </summary>
        public JunkbotGame()
        {
            Parameters = new JunkbotEngineParameters();
        }
        
        
        /// <inheritdoc />
        public void Begin(
            IEngineHost host
        )
        {
            Animations = new SpriteAnimationStore(Parameters);
            EngineHost = host;
            Running    = true;
            
            if (GameLoaded)
            {
                CurrentGameState = new SplashGameState(this);
            }
            else
            {
                // FIXME: Implement 'Select game' state
                //
                throw new NotImplementedException();
            }
        }
        
        /// <inheritdoc />
        public void RenderFrame(
            IGraphicsController graphics
        )
        {
            graphics.ClearViewport(Color.CornflowerBlue);

            CurrentGameState.RenderFrame(graphics);
        }
        
        /// <summary>
        /// Selects a Junkbot game to load, and if the engine is running, begin
        /// playing it.
        /// </summary>
        /// <param name="gameName">
        /// The name of the game to load.
        /// </param>
        public void SelectGame(
            string gameName
        )
        {
            // FIXME: Should validate whether the game data exists first
            //
            GameName = gameName;
            
            Levels      = new JunkbotLevelStore(this, gameName);
            UserProfile = JunkbotProfile.FromLevelSet(Levels);
            
            // If the game is running, then we need to back out from whatever the
            // current state is to the splash screen
            //
            if (Running)
            {
                var oldState = CurrentGameState;

                CurrentGameState = new SplashGameState(this);

                // oldState.Dispose();
            }
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
