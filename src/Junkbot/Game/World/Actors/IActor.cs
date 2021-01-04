/**
 * IActor.cs - Game Actor Interface
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Game.Actors.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Represents an actor in the game scene.
    /// </summary>
    internal interface IActor
    {
        /// <summary>
        /// Gets the animation server used by the actor.
        /// </summary>
        AnimationServer Animation { get; }
        
        /// <summary>
        /// Gets the bounding boxes of the actor.
        /// </summary>
        IList<Rectangle> BoundingBoxes { get; }
        
        /// <summary>
        /// Gets the size of the actor on the grid.
        /// </summary>
        Size GridSize { get; }
        
        /// <summary>
        /// Gets the location of the actor on the grid.
        /// </summary>
        Point Location { get; set; }
        
        
        /// <summary>
        /// Occurs when location of the actor has changed.
        /// </summary>
        event LocationChangedEventHandler LocationChanged;
        
        
        /// <summary>
        /// Update the specified deltaTime.
        /// </summary>
        /// <param name="deltaTime">
        /// The time difference since the last update.
        /// </param>
        void Update(
            TimeSpan deltaTime
        );
    }
}
