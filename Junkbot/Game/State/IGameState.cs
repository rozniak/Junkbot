using Junkbot.Game.Input;
using Junkbot.Game.World.Actors.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.State
{
    internal enum JunkbotGameState
    {
        Menu,
        Nothing,
        World
    }

    internal interface IGameState
    {
        JunkbotGameState Identifier { get; }


        bool Initialize(JunkbotGame gameReference, AnimationStore animationStore);

        void Update(TimeSpan deltaTime, InputEvents inputs);
    }
}
