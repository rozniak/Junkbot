using Junkbot.Game.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game
{
    internal delegate void GameStateChangedEventHandler(object sender, GameStateChangedEventArgs e);

    internal class GameStateChangedEventArgs : EventArgs
    {
        public IGameState NewState { get; private set; }


        public GameStateChangedEventArgs(IGameState newState)
        {
            NewState = newState;
        }
    }
}
