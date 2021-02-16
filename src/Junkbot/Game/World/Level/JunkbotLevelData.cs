/**
 * JunkbotLevelData.cs - Junkbot Level Data
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Game.World.Level
{
    /// <summary>
    /// Represents a data model for level data for Junkbot.
    /// </summary>
    public struct JunkbotLevelData
    {
        #region [info]
        
        /// <summary>
        /// Gets or sets the hint for the level.
        /// </summary>
        public string Hint { get; set; }
        
        /// <summary>
        /// Gets or sets the par number of moves for winning the level.
        /// </summary>
        public ushort Par { get; set; }
        
        /// <summary>
        /// Gets or sets the title of level.
        /// </summary>
        public string Title { get; set; }

        #endregion

        #region [background]
        
        /// <summary>
        /// Gets or sets the backdrop for the level.
        /// </summary>
        public string Backdrop { get; set; }
        
        /// <summary>
        /// Gets or sets the decals used in the backdrop of the level.
        /// </summary>
        public IList<JunkbotDecalData> Decals { get; set; }

        #endregion

        #region [playfield]
        
        /// <summary>
        /// Gets or sets the scale of the level.
        /// </summary>
        /// <remarks>
        /// Not sure if this is used... all levels seem to use 1 I think.
        /// </remarks>
        public byte Scale { get; set; }
        
        /// <summary>
        /// Gets or sets the size of the playfield in the level.
        /// </summary>
        public Size Size { get; set; }
        
        /// <summary>
        /// Gets or sets the cell size/grid spacing in the level.
        /// </summary>
        /// <remarks>
        /// This is always 15x18.
        /// </remarks>
        public Size Spacing { get; set; }

        #endregion

        #region [partslist]
        
        /// <summary>
        /// Gets or sets the colors used in the level.
        /// </summary>
        public string[] Colors { get; set; }
        
        /// <summary>
        /// Gets or sets the structure of parts in the level.
        /// </summary>
        /// <remarks>
        /// This is the actual layout of the level, ie. all the bricks, enemies, and
        /// Junkbot himself.
        /// </remarks>
        public IList<JunkbotPartData> Parts { get; set; }
        
        /// <summary>
        /// Gets or sets the types of parts used in the level.
        /// </summary>
        public string[] Types { get; set; }

        #endregion
    }
}
