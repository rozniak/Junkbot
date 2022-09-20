/**
 * JunkbotActorBase.cs - Junkbot Actor Base Class
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Game.Animation;
using System;
using System.Drawing;

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Represents the base class for Junkbot actors.
    /// </summary>
    public abstract class JunkbotActorBase
    {
        /// <summary>
        /// Gets the animation server used by the actor.
        /// </summary>
        public SpriteAnimationServer Animation { get; protected set; }
        
        /// <summary>
        /// Gets the bounding box of the actor.
        /// </summary>
        public abstract Rectangle BoundingBox { get; }
        
        /// <summary>
        /// Gets the size of the actor on the grid.
        /// </summary>
        public abstract Size GridSize { get; }

        /// <summary>
        /// Gets a value indicating whether the actor is mobile.
        /// </summary>
        public abstract bool IsMobile { get; }
        
        /// <summary>
        /// Gets the location of the actor on the grid.
        /// </summary>
        public Point Location
        {
            get { return _Location; }
            set
            {
                Point oldLocation = _Location;
                
                _Location = value;

                LocationChanged?.Invoke(
                    this,
                    new LocationChangedEventArgs(oldLocation, value)
                );
            }
        }
        private Point _Location;
        
        
        /// <summary>
        /// Occurs when location of the actor has changed.
        /// </summary>
        public event LocationChangedEventHandler LocationChanged;
        
        
        /// <summary>
        /// Gets the sprite data for the actor at the specified cell.
        /// </summary>
        /// <param name="x">
        /// The cell x-coordinate.
        /// </param>
        /// <param name="y">
        /// The cell y-coordinate.
        /// </param>
        /// <returns>
        /// The sprite data to use to draw the actor or part of the actor that
        /// occupies the cell, null if no sprite should be drawn.
        /// </returns>
        public virtual SpriteAnimationSpriteData GetSpriteAtCell(
            int x,
            int y
        )
        {
            // Get sprite from frame - split into vertical chunks
            //
            if (x != Location.X)
            {
                return null;
            }

            SpriteAnimationFrame frame       = Animation.GetCurrentFrame();
            int                  spriteIndex = y - Location.Y;

            return frame.Sprites[spriteIndex];
        }

        /// <summary>
        /// Update the specified deltaTime.
        /// </summary>
        /// <param name="deltaTime">
        /// The time difference since the last update.
        /// </param>
        public abstract void Update(
            TimeSpan deltaTime
        );
    }
}
