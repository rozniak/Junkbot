/**
 * BrickSize.cs - Junkbot Lego Brick Size Enumeration
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Specifies constants defining possible sizes for Lego bricks.
    /// </summary>
    internal enum BrickSize
    {
        /// <summary>
        /// The brick is one cell wide.
        /// </summary>
        One   = 1,
        
        /// <summary>
        /// The brick is two cells wide.
        /// </summary>
        Two   = 2,
        
        /// <summary>
        /// The brick is three cells wide.
        /// </summary>
        Three = 3,
        
        /// <summary>
        /// The brick is four cells wide.
        /// </summary>
        Four  = 4,
        /// <summary>
        /// The brick is six cells wide.
        /// </summary>
        Six   = 6,
        
        /// <summary>
        /// The brick is eight cells wide.
        /// </summary>
        Eight = 8
    }
}
