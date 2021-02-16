/**
 * FacingDirection.cs - Junkbot Facing Direction Enumeration
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Specifies constants defining possible directions for actors to face.
    /// </summary>
    public enum FacingDirection
    {
        /// <summary>
        /// The actor is facing down.
        /// </summary>
        Down,
        
        /// <summary>
        /// The actor is facing left.
        /// </summary>
        Left,
        
        /// <summary>
        /// The actor is facing right.
        /// </summary>
        Right,
        
        /// <summary>
        /// The actor is facing up.
        /// </summary>
        Up
    }
}
