/**
 * BrickActor.cs - Junkbot Lego Brick Actor
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Game.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Represents a Lego brick in Junkbot.
    /// </summary>
    public sealed class BrickActor : JunkbotActorBase
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
        public override Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(Location, GridSize);
            }
        }

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
        public override Size GridSize
        {
            get
            {
                return new Size(
                    (int) Size,
                    1
                );
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
        
        /// <summary>
        /// Gets the size of the brick.
        /// </summary>
        public BrickSize Size
        {
            get { return _Size; }
            set
            {
                _Size    = value;
                UpdateBrickAnim();
            }
        }
        private BrickSize _Size;
        
        
        /// <summary>
        /// The scene.
        /// </summary>
        private Scene Scene { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="BrickActor"/> class.
        /// </summary>
        /// <param name="scene">
        /// The scene.
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
        /// <param name="store">
        /// The animation store.
        /// </param>
        public BrickActor(
            Scene                scene,
            Point                location,
            Color                color,
            BrickSize            size,
            SpriteAnimationStore store = null
        )
        {
            Location                 = location;
            _Color                   = color;
            _Size                    = size;
            
            if (store != null)
            {
                Animation = new SpriteAnimationServer(store);
                
                UpdateBrickAnim();
            }
        }
        
        
        /// <inheritdoc />
        public override SpriteAnimationSpriteData GetSpriteAtCell(
            int x,
            int y
        )
        {
            if (
                x != Location.X ||
                y != Location.Y
            )
            {
                return null;
            }

            return Animation.GetCurrentFrame().Sprites[0];
        }

        /// <inheritdoc />
        public override void Update(
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
