/**
 * BrickDetachDirection.cs - Junkbot Lego Brick Detach Direction Enumeration
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

namespace Junkbot.Game.World.Logic
{
    /// <summary>
    /// Specifies constants defining possible directions to detach bricks.
    /// </summary>
    public enum BrickDetachDirection
    {
        /// <summary>
        /// The brick should detach downwards.
        /// </summary>
        Downwards,
        
        /// <summary>
        /// The brick can be detached in either direction.
        /// </summary>
        Either,
        
        /// <summary>
        /// The brick should detach upwards.
        /// </summary>
        Upwards
    }
}