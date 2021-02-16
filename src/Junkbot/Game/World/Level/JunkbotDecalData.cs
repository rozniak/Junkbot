/**
 * JunkbotDecalData.cs - Junkbot Level Decal Data
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
    /// Represets a data model for decal data within Junkbot levels.
    /// </summary>
    public struct JunkbotDecalData
    {
        /// <summary>
        /// Gets or sets the location of the decal.
        /// </summary>
        public Point Location { get; set; }
        
        /// <summary>
        /// Gets or sets the sprite name of the decal.
        /// </summary>
        public string SpriteName { get; set; }
    }
}
