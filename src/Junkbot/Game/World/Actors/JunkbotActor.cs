/**
 * JunkbotActor.cs - Junkbot Actor
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Extensions;
using Oddmatics.Rzxe.Game.Animation;
using System;
using System.Drawing;

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Represents Junkbot himself.
    /// </summary>
    public sealed class JunkbotActor : JunkbotActorBase
    {
        /// <summary>
        /// The bounding box of Junkbot.
        /// </summary>
        private static readonly Rectangle JunkbotBoundingBox =
            new Rectangle(0, 0, 2, 4);
    
        /// <summary>
        /// The size of the Junkbot on the grid.
        /// </summary>
        private static readonly Size JunkbotGridSize = new Size(2, 4);
        
        
        /// <inheritdoc />
        public override Rectangle BoundingBox
        {
            get
            {
                return JunkbotBoundingBox.AddOffset(Location);
            }
        }
        
        /// <inheritdoc />
        public override Size GridSize
        {
            get { return JunkbotGridSize; }
        }
        
        
        /// <summary>
        /// The current brain state.
        /// </summary>
        private JunkbotBrainState BrainState { get; set; }
        
        /// <summary>
        /// The delegate method that executes the next step for the current brain
        /// state.
        /// </summary>
        private Action BrainStateCallback { get; set; }

        /// <summary>
        /// The direction that Junkbot is currently facing.
        /// </summary>
        private FacingDirection FacingDirection { get; set; }

        /// <summary>
        /// The scene.
        /// </summary>
        private Scene Scene { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotActor"/> class.
        /// </summary>
        /// <param name="scene">
        /// The scene.
        /// </param>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <param name="initialDirection">
        /// The initial direction for Junkbot to face.
        /// </param>
        /// <param name="store">
        /// The animation store.
        /// </param>
        public JunkbotActor(
            Scene                scene,
            Point                location,
            FacingDirection      initialDirection,
            SpriteAnimationStore store = null
        )
        {
            Location  = location;
            Scene     = scene;
            
            if (store != null)
            {
                Animation = new SpriteAnimationServer(store);
                
                SetBrainState(
                    JunkbotBrainState.Walking,
                    initialDirection
                );
            }
            else
            {
                FacingDirection = initialDirection;
            }
        }
        
        
        /// <inheritdoc />
        public override void Update(
            TimeSpan deltaTime
        )
        {
            Animation.Progress(deltaTime);
        }
        
        
        /// <summary>
        /// Sets Junkbot's brain state.
        /// </summary>
        /// <param name="state">
        /// The new state.
        /// </param>
        /// <param name="direction">
        /// The direction Junkbot should face.
        /// </param>
        private void SetBrainState(
            JunkbotBrainState state,
            FacingDirection   direction = FacingDirection.Right
        )
        {
            // Detach event if necessary
            //
            try
            {
                Animation.SpecialFrameEntered -= Animation_SpecialFrameEntered;
            }
            catch (Exception ex) { }

            FacingDirection = direction;
            
            switch (state)
            {
                case JunkbotBrainState.Walking:
                    BrainStateCallback = StepWalkingState;
                    
                    switch (direction)
                    {
                        case FacingDirection.Left:
                            Animation.GoToAndPlay("junkbot-walk-left");
                            break;
                        
                        case FacingDirection.Right:
                            Animation.GoToAndPlay("junkbot-walk-right");
                            break;

                        default:
                            throw new ArgumentException(
                                "Invalid direction for Junkbot."
                            );
                    }

                    break;

                default:
                    throw new NotImplementedException(
                        "State not implemented."
                    );
            }

            Animation.SpecialFrameEntered += Animation_SpecialFrameEntered;
        }
        
        /// <summary>
        /// Turns Junkbot around to begin walking the opposite direction.
        /// </summary>
        private void TurnAround()
        {
            SetBrainState(
                JunkbotBrainState.Walking,
                FacingDirection == FacingDirection.Left ?
                    FacingDirection.Right :
                    FacingDirection.Left
            );
        }


        /// <summary>
        /// Executes the next step in the walking state.
        /// </summary>
        private void StepWalkingState()
        {
            Rectangle rect = BoundingBox;
            int       x    = FacingDirection == FacingDirection.Left ?
                               Location.X + 1 :
                               Location.X;
            int       y    = Location.Y + GridSize.Height;
            int       dx   = FacingDirection == FacingDirection.Left ? -1 : 1;
            int       newX = x + dx;
            
            // Check floor elevation in front of Junkbot
            //
            // We need to check in this order:
            //     - 1 space forwards and up (stair upwards), free space above
            //     - 2 spaces fowards (no elevation - Junkbot can skip 1 cell gaps)
            //     - 1 space forwards (no elevation)
            //     - 1 space forwards and down (stair downwards)
            //
            JunkbotActorBase floor = null;
            int              dy    = 0;
            
            if ((floor = Scene.GetActorAtCell(x + (dx * 2), y - 1)) != null)
            {
                dy = -1;
            }
            else
            {
                if (
                    (floor = Scene.GetActorAtCell(newX, y))         != null ||
                    (floor = Scene.GetActorAtCell(x + (dx * 2), y)) != null
                )
                {
                    dy = 0;
                }
                else
                {
                    if ((floor = Scene.GetActorAtCell(newX, y + 1)) != null)
                    {
                        dy = 1;
                    }
                    else
                    {
                        // No floor ahead... turn around!
                        //
                        TurnAround();
                    }
                }
            }
            
            if (!(floor is BrickActor))
            {
                TurnAround();
            }

            // Check target is free
            //
            Rectangle targetBounds = rect.AddOffset(new Point(dx, dy));
            
            if (!Scene.CheckGridRegionFreeForActor(this, targetBounds))
            {
                TurnAround();
                return;
            }
            
            Location = targetBounds.Location;
        }


        /// <summary>
        /// (Event) Handles when an event is emitted as part of an animation.
        /// </summary>
        private void Animation_SpecialFrameEntered(
            object    sender,
            EventArgs e
        )
        {
            BrainStateCallback();
        }
    }
}
