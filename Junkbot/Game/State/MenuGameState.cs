using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.State
{
    internal class MenuGameState : IGameState
    {
        public JunkbotGameState Identifier { get { return JunkbotGameState.Menu; } }


        private JunkbotGame Game;


        public MenuGameState()
        {

        }


        public bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;

            return true;
        }

        public void Update(TimeSpan deltaTime)
        {

        }
    }
}
