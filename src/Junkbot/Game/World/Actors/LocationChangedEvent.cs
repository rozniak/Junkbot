using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors
{
    internal delegate void LocationChangedEventHandler(object sender, LocationChangedEventArgs e);

    internal class LocationChangedEventArgs : EventArgs
    {
        public Point OldLocation { get; private set; }

        public Point NewLocation { get; private set; }


        public LocationChangedEventArgs(Point oldLocation, Point newLocation)
        {
            OldLocation = oldLocation;
            NewLocation = newLocation;
        }
    }
}
