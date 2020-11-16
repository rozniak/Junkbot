using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors
{
    interface IThinker : IActor
    {
        void Think(TimeSpan deltaTime);
    }
}
