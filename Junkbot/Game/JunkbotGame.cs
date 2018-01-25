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
        public JunkbotGameState GameState
        {
            get { return _GameState; }
            private set
            {
                _GameState = value;
                ChangeState?.Invoke(this, EventArgs.Empty);
            }
        }
        private JunkbotGameState _GameState;


        private Scene GameScene;
        

        public event EventHandler ChangeState;


        public JunkbotGame()
        {
            GameScene = new Scene();
            GameState = JunkbotGameState.Nothing;
        }


        public void Update(TimeSpan deltaTime)
        {
            if (GameState == JunkbotGameState.Nothing)
                GameState = JunkbotGameState.World;
        }
    }
}
