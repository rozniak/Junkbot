using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors.Animation
{
    internal class ActorAnimationFrame
    {
        public Point Offset { get; private set; }

        public bool ShouldEmitEvent { get; private set; }

        public string SpriteName { get; private set; }

        public byte Ticks { get; private set; }


        public ActorAnimationFrame(bool shouldEmitEvent, Point offset, string spriteName, byte ticks)
        {
            ShouldEmitEvent = shouldEmitEvent;
            Offset = offset;
            SpriteName = spriteName;
            Ticks = ticks;
        }
    }
}
