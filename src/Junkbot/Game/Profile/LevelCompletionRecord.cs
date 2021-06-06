/**
 * LevelCompletionRecord.cs - Junkbot Level Completion Record.
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Newtonsoft.Json;

namespace Junkbot.Game.Profile
{
    /// <summary>
    /// Represents data regarding level completion information.
    /// </summary>
    public class LevelCompletionRecord
    {
        /// <summary>
        /// Gets or sets the value that indicates
        /// </summary>
        [JsonProperty(PropertyName = "done")]
        public bool Done { get; set; }
        
        /// <summary>
        /// Gets or sets the moves.
        /// </summary>
        [JsonProperty(PropertyName = "moves")]
        public int Moves { get; set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelCompletionRecord"/>
        /// class.
        /// </summary>
        public LevelCompletionRecord()
        {
            Done  = false;
            Moves = 0;
        }
    }
}
