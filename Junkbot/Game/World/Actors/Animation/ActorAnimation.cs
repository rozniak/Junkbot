using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors.Animation
{
    internal class ActorAnimation
    {
        public ActorAnimationFrame CurrentFrame { get; private set; }


        public event EventHandler FinishedPlayback;


        public ActorAnimation()
        {

        }
    }
}
