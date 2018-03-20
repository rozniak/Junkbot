using Junkbot.Game.Input;
using Junkbot.Game.UI;
using Junkbot.Game.World.Actors.Animation;
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

        public JunkbotInterface Interface { get; private set; }


        private AnimationStore AnimationStore;

        private JunkbotGame Game;


        public bool Initialize(JunkbotGame gameReference, AnimationStore animationStore)
        {
            Game = gameReference;
            Interface = new JunkbotInterface();

            return true;
        }

        public void Update(TimeSpan deltaTime, InputEvents inputs)
        {
            
        }
    }
}
