/**
 * JunkbotPartData.cs - Junkbot Level Part Data
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using System.Drawing;

namespace Junkbot.Game.World.Level
{
    /// <summary>
    /// Represents a data model for part data within Junkbot levels.
    /// </summary>
    public class JunkbotPartData
    {
        /// <summary>
        /// Gets or sets the name of the animation to start the actor on.
        /// </summary>
        public string AnimationName { get; set; }

        /// <summary>
        /// Gets or sets the index of the part color for the actor.
        /// </summary>
        public byte ColorIndex;
        
        /// <summary>
        /// Gets or sets the location of the actor in the level.
        /// </summary>
        public Point Location;
        
        /// <summary>
        /// Gets or sets the index of the part type for the actor.
        /// </summary>
        public byte TypeIndex;
    }
}
