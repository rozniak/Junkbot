/**
 * GreyBrickPathState.cs - Junkbot Grey Brick Path State
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Specifies constants defining whether or not a given brick is connected in a
    /// way that it has a direct path to a grey brick.
    /// </summary>
    public enum GreyBrickPathState
    {
        /// <summary>
        /// The brick has direct paths to a grey brick in either direction.
        /// </summary>
        EitherPath,
        
        /// <summary>
        /// The brick has no direct path to a grey brick.
        /// </summary>
        NoPath,
        
        /// <summary>
        /// The brick has a direct path downwards to a grey brick.
        /// </summary>
        PathDownwards,
        
        /// <summary>
        /// The brick has a direct path upwards to a grey brick.
        /// </summary>
        PathUpwards
    }
}
