using Junkbot.Game.State;
using Junkbot.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game
{
    internal class JunkbotGame
    {
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
        

        public event EventHandler ChangeState;


        public JunkbotGame()
        {
            // Load straight into the menu gamestate for now
            //
            var menuState = new MenuGameState();

            menuState.Initialize(this);

            GameState = menuState;
        }


        public void Update(TimeSpan deltaTime)
        {
            GameState.Update(deltaTime);
        }
    }
}
