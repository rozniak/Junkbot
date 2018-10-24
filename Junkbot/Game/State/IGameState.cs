using Junkbot.Game.Input;
using Junkbot.Game.World.Actors.Animation;
using System;

namespace Junkbot.Game.State
{
    /// <summary>
    /// Specifies constants that identify the game state.
    /// </summary>
    internal enum JunkbotGameState
    {
        Menu,
        Nothing,
        World
    }


    /// <summary>
    /// Represents a game state within the Junkbot game engine.
    /// </summary>
    internal interface IGameState
    {
        /// <summary>
        /// Gets the value that determines the game state this is.
        /// </summary>
        JunkbotGameState Identifier { get; }

        /// <summary>
        /// Gets the Junkbot game scene.
        /// </summary>
        Scene Scene { get; }


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
        bool Initialize(JunkbotGame gameReference, AnimationStore animationStore);

        /// <summary>
        /// Updates this game state.
        /// </summary>
        /// <param name="deltaTime">The time difference since the last update.</param>
        /// <param name="inputs">The input events that have occurred.</param>
        void Update(TimeSpan deltaTime, InputEvents inputs);
    }
}
