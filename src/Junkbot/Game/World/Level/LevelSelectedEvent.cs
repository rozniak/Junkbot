/**
 * LevelSelectedEvent.cs - Junkbot Level Selected Event
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.Interface;
using System;

namespace Junkbot.Game.World.Level
{
    /// <summary>
    /// Represents the method that will handle the
    /// <see cref="JunkbotUxLevelList.LevelSelected"/> event.
    /// </summary>
    /// <param name="sender">
    /// The source of the event.
    /// </param>
    /// <param name="e">
    /// A <see cref="LevelSelectedEventArgs"/> that contains event data.
    /// </param>
    public delegate void LevelSelectedEventHandler(
        object                 sender,
        LevelSelectedEventArgs e
    );
    
    
    /// <summary>
    /// Provides data for the <see cref="JunkbotUxLevelList.LevelSelected"/> event.
    /// </summary>
    public class LevelSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the index of the building that was selected.
        /// </summary>
        public byte BuildingIndex { get; private set; }
        
        /// <summary>
        /// Gets the index of the level that was selected.
        /// </summary>
        public byte LevelIndex { get; private set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelSelectedEventArgs"/>
        /// class.
        /// </summary>
        public LevelSelectedEventArgs(
            byte buildingIndex,
            byte levelIndex
        )
        {
            BuildingIndex = buildingIndex;
            LevelIndex    = levelIndex;
        }
    }
}
