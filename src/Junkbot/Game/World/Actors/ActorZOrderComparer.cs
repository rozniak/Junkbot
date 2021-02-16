/**
 * ActorZOrderComparer.cs - Comparer for Sorting Junkbot Actors by Z-Order
 *
 * This source-code is part of rzxe - an experimental game engine by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using System.Collections.Generic;

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Represents a comparison operation for sorting Junkbot actors by z-order that
    /// uses their position on the grid.
    /// </summary>
    public class ActorZOrderComparer : IComparer<JunkbotActorBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActorZOrderComparer"/> class.
        /// </summary>
        public ActorZOrderComparer() { }
        
        
        /// <inheritdoc />
        public int Compare(
            JunkbotActorBase x,
            JunkbotActorBase y
        )
        {
            // Ordering bottom to top, then left to right
            //
            if (x.Location.Y < y.Location.Y)
            {
                return 1;
            }
            else if (x.Location.Y > y.Location.Y)
            {
                return -1;
            }
            else // Same Y axis
            {
                if (x.Location.X < y.Location.X)
                {
                    return -1;
                }
                else if (x.Location.X > y.Location.X)
                {
                    return 1;
                }
            }
            
            return 0; // Should not occur really - objects in the same spot
        }
    }
}
