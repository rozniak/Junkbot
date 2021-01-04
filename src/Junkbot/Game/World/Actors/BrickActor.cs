/**
 * BrickActor.cs - Junkbot Lego Brick Actor
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
    /// Represents a Lego brick in Junkbot.
    /// </summary>
    internal class BrickActor : IActor
    {
        /// <summary>
        /// The valid colors for Lego bricks in Junkbot.
        /// </summary>
        public static IList<Color> ValidColors =
            new List<Color>(
                new Color[]
                {
                    Color.Red, Color.Yellow, Color.White, Color.Green, Color.Blue
                }
            ).AsReadOnly();
        
        
        /// <inheritdoc />
        public AnimationServer Animation { get; private set; }
        
        /// <inheritdoc />
        public IList<Rectangle> BoundingBoxes { get; private set; }

        /// <summary>
        /// Gets the color of the brick.
        /// </summary>
        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                UpdateBrickAnim();
            }
        }
        private Color _Color;
        
        /// <inheritdoc />
        public Size GridSize
        {
            get { return _GridSize; }
            private set
            {
                _GridSize = value;
                
                BoundingBoxes =
                    new List<Rectangle>()
                    {
                        new Rectangle(
                            Point.Empty,
                            GridSize
                        )
                    }.AsReadOnly();
            }
        }
        private Size _GridSize;

        /// <summary>
        /// Gets a value indicating whether the brick is immobile.
        /// </summary>
        public bool IsImmobile
        {
            get { return _Color.Name == "Gray"; }
        }
        
        /// <inheritdoc />
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
        /// Gets the size of the brick.
        /// </summary>
        public BrickSize Size
        {
            get { return _Size; }
            set
            {
                _Size    = value;
                GridSize = new Size((int) value, 1);
                
                UpdateBrickAnim();
            }
        }
        private BrickSize _Size;
        
        
        /// <inheritdoc />
        public event LocationChangedEventHandler LocationChanged;
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BrickActor"/> class.
        /// </summary>
        /// <param name="store">
        /// The animation store.
        /// </param>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <param name="color">
        /// The color of the brick.
        /// </param>
        /// <param name="size">
        /// The size of the brick.
        /// </param>
        public BrickActor(
            AnimationStore store,
            Point          location,
            Color          color,
            BrickSize      size
        )
        {
            Animation = new AnimationServer(store);
            GridSize  = new Size((int) size, 1);
            Location  = location;
            _Color    = color;
            _Size     = size;

            UpdateBrickAnim();
        }
        
        
        /// <inheritdoc />
        public void Update(
            TimeSpan deltaTime
        )
        {
            Animation.Progress(deltaTime);
        }
        
        
        /// <summary>
        /// Updates the brick animation.
        /// </summary>
        private void UpdateBrickAnim()
        {
            string brickSize = ((int) Size).ToString();

            if (IsImmobile)
            {
                Animation.GoToAndStop(
                    "legopart-brick-immobile-" + brickSize
                );
            }
            else
            {
                if (!ValidColors.Contains(Color))
                {
                    return;
                }

                Animation.GoToAndStop(
                    "legopart-brick-" + Color.Name.ToLower() + "-" + brickSize
                );
            }
        }
    }
}
