using Junkbot.Game.State;
using System;

namespace Junkbot.Game
{
    /// <summary>
    /// Represents the method that will handle the
    /// <see cref="JunkbotGame.ChangeState"/> event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An object that contains event data.</param>
    internal delegate void GameStateChangedEventHandler(object sender, GameStateChangedEventArgs e);


    /// <summary>
    /// Event arguments for the <see cref="JunkbotGame.ChangeState"/> event.
    /// </summary>
    internal class GameStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the game state that has been switched to.
        /// </summary>
        public IGameState NewState { get; private set; }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="GameStateChangedEventArgs"/>
        /// class with event data.
        /// </summary>
        /// <param name="newState">The game state that has been switched to.</param>
        public GameStateChangedEventArgs(IGameState newState)
        {
            NewState = newState;
        }
    }
}
