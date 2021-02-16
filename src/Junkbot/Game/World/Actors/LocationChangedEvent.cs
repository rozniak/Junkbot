/**
 * LocationChangedEvent.cs - Actor Location Changed Event
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using System;
using System.Drawing;

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Represents the method that will handle the
    /// <see cref="JunkbotActorBase.LocationChanged"/> event.
    /// </summary>
    /// <param name="sender">
    /// The source of the event.
    /// </param>
    /// <param name="e">
    /// An <see cref="LocationChangedEventArgs"/> object that contains event data.
    /// </param>
    public delegate void LocationChangedEventHandler(
        object                   sender,
        LocationChangedEventArgs e
    );
    
    
    /// <summary>
    /// Provides data for the <see cref="JunkbotActorBase.LocationChanged"/> event.
    /// </summary>
    public class LocationChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new location.
        /// </summary>
        public Point NewLocation { get; private set; }
        
        /// <summary>
        /// Gets the old location.
        /// </summary>
        public Point OldLocation { get; private set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationChangedEventArgs"/>.
        /// </summary>
        /// <param name="oldLocation">
        /// The old location.
        /// </param>
        /// <param name="newLocation">
        /// The new location.
        /// </param>
        public LocationChangedEventArgs(
            Point oldLocation,
            Point newLocation
        )
        {
            OldLocation = oldLocation;
            NewLocation = newLocation;
        }
    }
}
