/**
 * JunkbotLevelListing.cs - Junkbot Level Listing
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Newtonsoft.Json;

namespace Junkbot.Game.World.Level
{
    /// <summary>
    /// Represents the level listing for a Junkbot game.
    /// </summary>
    public struct JunkbotLevelListing
    {
        /// <summary>
        /// Gets or sets the number of buildings in the game.
        /// </summary>
        [JsonProperty(PropertyName = "buildings")]
        public int Buildings { get; set; }
        
        /// <summary>
        /// Gets or sets the level names.
        /// </summary>
        [JsonProperty(PropertyName = "levels")]
        public string[] Levels { get; set; }

        /// <summary>
        /// Gets or sets the name of the level used on the splash screen.
        /// </summary>
        [JsonProperty(PropertyName = "splash-level")]
        public string SplashLevel { get; set; }
    }
}
