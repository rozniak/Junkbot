using Junkbot.Game.World.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Helpers
{
    internal class BrickPositionComparer : IComparer<BrickActor>
    {
        public int Compare(BrickActor x, BrickActor y)
        {
            if (x.Location.Y < y.Location.Y)
            {
                return 1;
            }
            else if (x.Location.Y > y.Location.Y)
            {
                return -1;
            }
            else
            {
                if (x.Location.X > y.Location.X)
                {
                    return 1;
                }
                else if (x.Location.X < y.Location.X)
                {
                    return -1;
                }
            }

            return 0;
        }
    }

    internal static class ListHelper
    {
        public static void InsertSorted(this List<BrickActor> subject, BrickActor brick)
        {
            int index = subject.BinarySearch(brick, new BrickPositionComparer());
            subject.Insert((index >= 0) ? index : ~index, brick);
        }
    }
}
