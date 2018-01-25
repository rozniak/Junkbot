using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors
{
    internal class JunkbotActor : IThinker
    {
        public Vector2 Location { get; set; }


        public JunkbotActor()
        {
            Location = Vector2.Zero;
        }


        public void Think(TimeSpan deltaTime)
        {

        }
    }
}
