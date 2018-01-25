using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors
{
    internal class BrickActor : IActor
    {
        public BrickColor Color;


        public BrickActor()
        {
            Color = BrickColor.Blue;
        }
    }
}
