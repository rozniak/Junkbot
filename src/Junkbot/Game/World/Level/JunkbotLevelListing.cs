/**
 * JunkbotLevelListing.cs - Junkbot Level Listing
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

namespace Junkbot.Game.World.Level
{
    /// <summary>
    /// Represents the level listing for a Junkbot game.
    /// </summary>
    internal struct JunkbotLevelListing
    {
        /// <summary>
        /// Gets or sets the level names.
        /// </summary>
        public string[] Levels { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the level used on the splash screen.
        /// </summary>
        public string SplashLevel { get; set; }
    }
}
