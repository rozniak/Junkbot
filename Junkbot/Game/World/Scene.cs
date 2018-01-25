using Junkbot.Game.World.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World
{
    internal class Scene
    {
        public List<BrickActor> Bricks { get; private set; }

        public JunkbotActor Junkbot { get; private set; }


        public Scene()
        {
            Bricks = new List<BrickActor>();
            Junkbot = new JunkbotActor();
        }
    }
}
