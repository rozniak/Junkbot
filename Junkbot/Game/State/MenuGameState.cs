using Junkbot.Game.Input;
using Junkbot.Game.UI;
using Junkbot.Game.World.Actors.Animation;
using System;

namespace Junkbot.Game.State
{
    /// <summary>
    /// Represents the main menu game state.
    /// </summary>
    internal class MenuGameState : IGameState
    {
        /// <summary>
        /// Gets the value that determines the game state this is.
        /// </summary>
        public JunkbotGameState Identifier { get { return JunkbotGameState.Menu; } }

        /// <summary>
        /// Gets the interface manager used in this game state.
        /// </summary>
        public JunkbotInterface Interface { get; private set; }


        /// <summary>
        /// The animation store repository.
        /// </summary>
        private AnimationStore AnimationStore;

        /// <summary>
        /// The Junkbot game engine.
        /// </summary>
        private JunkbotGame Game;


        /// <summary>
        /// Initializes this <see cref="IGameState"/> with references to the game
        /// engine and animation store repository instances.
        /// </summary>
        /// <param name="gameReference">
        /// A reference to the game engine instance.
        /// </param>
        /// <param name="animationStore">
        /// A reference to the animation store repository instance.
        /// </param>
        /// <returns>True if the initialization routine was successful.</returns>
        public bool Initialize(JunkbotGame gameReference, AnimationStore animationStore)
        {
            Game = gameReference;
            Interface = new JunkbotInterface();

            return true;
        }

        /// <summary>
        /// Updates this game state.
        /// </summary>
        /// <param name="deltaTime">The time difference since the last update.</param>
        /// <param name="inputs">The input events that have occurred.</param>
        public void Update(TimeSpan deltaTime, InputEvents inputs)
        {
            
        }
    }
}
