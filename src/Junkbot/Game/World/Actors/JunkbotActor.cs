/**
 * JunkbotActor.cs - Junkbot Actor
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Helpers;
using Oddmatics.Rzxe.Game.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Represents Junkbot himself.
    /// </summary>
    internal class JunkbotActor : JunkbotActorBase
    {
        /// <summary>
        /// The bounding boxes of Junkbot.
        /// </summary>
        private static readonly IList<Rectangle> JunkbotBoundingBoxes =
            new List<Rectangle>(new Rectangle[]
            {
                new Rectangle(0, 0, 2, 3),
                new Rectangle(0, 3, 1, 1)
            }).AsReadOnly();
    
        /// <summary>
        /// The size of the Junkbot on the grid.
        /// </summary>
        private static readonly Size JunkbotGridSize = new Size(2, 4);
        
        
        /// <inheritdoc />
        public override IList<Rectangle> BoundingBoxes
        {
            get { return JunkbotBoundingBoxes; }
        }
        
        /// <inheritdoc />
        public override Size GridSize
        {
            get { return JunkbotGridSize; }
        }


        /// <summary>
        /// The direction that Junkbot is currently facing.
        /// </summary>
        private FacingDirection FacingDirection;
        
        /// <summary>
        /// The scene.
        /// </summary>
        private Scene Scene;
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotActor"/> class.
        /// </summary>
        /// <param name="store">
        /// The animation store.
        /// </param>
        /// <param name="scene">
        /// The scene.
        /// </param>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <param name="initialDirection">
        /// The initial direction for Junkbot to face.
        /// </param>
        public JunkbotActor(
            SpriteAnimationStore store,
            Scene                scene,
            Point                location,
            FacingDirection      initialDirection
        )
        {
            Animation = new SpriteAnimationServer(store);
            Location  = location;
            Scene     = scene;
            
            SetWalkingDirection(initialDirection);
        }
        
        
        /// <inheritdoc />
        public override void Update(
            TimeSpan deltaTime
        )
        {
            Animation.Progress(deltaTime);
        }
        
        
        /// <summary>
        /// Sets Junkbot's walking direction.
        /// </summary>
        /// <param name="direction">
        /// The new direction.
        /// </param>
        private void SetWalkingDirection(
            FacingDirection direction
        )
        {
            FacingDirection = direction;

            // Detach event if necessary
            //
            try
            {
                Animation.SpecialFrameEntered -= Animation_SpecialFrameEntered;
            }
            catch (Exception ex) { }

            switch (direction)
            {
                case FacingDirection.Left:
                    Animation.GoToAndPlay("junkbot-walk-left");
                    break;
                
                case FacingDirection.Right:
                    Animation.GoToAndPlay("junkbot-walk-right");
                    break;
                
                default:
                    throw new Exception("Invalid direction provided.");
            }

            Animation.SpecialFrameEntered += Animation_SpecialFrameEntered;
        }
        
        
        /// <summary>
        /// (Event) 
        /// </summary>
        private void Animation_SpecialFrameEntered(
            object    sender,
            EventArgs e
        )
        {
            int dx = FacingDirection == FacingDirection.Left ? -1 : 1;
            
            // Check if we should turn around now
            //
            var checkBounds =
                new Rectangle(
                    Location.Add(new Point(dx * GridSize.Width, 0)),
                    new Size(1, 3)
                );
            
            if (!Scene.CheckGridRegionFree(checkBounds))
            {
                Location = Location.Add(new Point(dx, 0));

                SetWalkingDirection(
                    FacingDirection == FacingDirection.Left ?
                                         FacingDirection.Right :
                                         FacingDirection.Left
                );
                
                return;
            }
            
            // Space is free, now check whether we need an elevation change, prioritize
            // upwards changes
            //
            var floorUpCheckBounds =
                new Rectangle(
                    Location.Add(new Point(dx, GridSize.Height - 1)),
                    new Size(1, 1)
                );
            
            if (!Scene.CheckGridRegionFree(floorUpCheckBounds))
            {
                // Elevate up
                //
                Location = Location.Add(new Point(dx, -1));
                return;
            }
            
            // Now check downwards
            //
            var floorMissingCheckBounds =
                new Rectangle(
                    Location.Add(new Point(dx, GridSize.Height)),
                    new Size(1, 1)
                );
            
            var floorDownCheckBounds =
                new Rectangle(
                    Location.Add(new Point(dx, GridSize.Height + 1)),
                    new Size(1, 1)
                );
            
            if (
                Scene.CheckGridRegionFree(floorMissingCheckBounds) &&
                !Scene.CheckGridRegionFree(floorDownCheckBounds))
            {
                // Lower junkbot
                //
                Location = Location.Add(new Point(dx, 1));
                return;
            }
            
            Location = Location.Add(new Point(dx, 0));
        }
    }
}
