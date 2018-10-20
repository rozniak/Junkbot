using Junkbot.Game.Input;
using Junkbot.Game.State;
using Junkbot.Game.World.Actors.Animation;
using System;

namespace Junkbot.Game
{
    /// <summary>
    /// Represents the main Junkbot game engine.
    /// </summary>
    internal sealed class JunkbotGame
    {
        /// <summary>
        /// Gets or sets the active game state.
        /// </summary>
        public IGameState GameState
        {
            get { return _GameState; }
            private set
            {
                _GameState = value;
                ChangeState?.Invoke(this, EventArgs.Empty);
            }
        }
        private IGameState _GameState;
        
        
        /// <summary>
        /// Occurs when the active game state has changed.
        /// </summary>
        public event EventHandler ChangeState;


        /// <summary>
        /// The animation store repository.
        /// </summary>
        private AnimationStore AnimationStore;


        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotGame"/> class.
        /// </summary>
        public JunkbotGame()
        {
            AnimationStore = new AnimationStore();
        }


        /// <summary>
        /// Begins running the Junkbot game engine.
        /// </summary>
        public void Begin()
        {
            // Load straight into the menu gamestate for now
            //
            var menuState = new MenuGameState();

            menuState.Initialize(this, AnimationStore);

            GameState = menuState;
        }
        
        /// <summary>
        /// Updates the state of the Junkbot game engine.
        /// </summary>
        /// <param name="deltaTime">The time difference since the last update.</param>
        /// <param name="inputs">The input events that have occurred.</param>
        public void Update(TimeSpan deltaTime, InputEvents inputs)
        {
            GameState.Update(deltaTime, inputs);
        }
    }
}
