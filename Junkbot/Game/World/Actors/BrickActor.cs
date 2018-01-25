using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors
{
    internal class BrickActor : IActor
    {
        public BrickColor Color { get; set; }

        public Vector2 Location { get; set; }


        public BrickActor()
        {
            Color = BrickColor.Blue;
            Location = Vector2.Zero;
        }
    }
}
