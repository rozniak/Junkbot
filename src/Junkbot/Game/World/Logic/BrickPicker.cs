/**
 * BrickPicker.cs - Junkbot Lego Brick Picker
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.World.Actors;
using Oddmatics.Rzxe.Extensions;
using Oddmatics.Rzxe.Game.Animation;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Logic;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Junkbot.Game.World.Logic
{
    /// <summary>
    /// Provides functionality for mapping brick connections and picking up bricks.
    /// </summary>
    public sealed class BrickPicker
    {
        /// <summary>
        /// Approximately one tick for Junkbot, which runs at 4 frames at 60FPS.
        /// </summary>
        private const double ApproxOneTick = 64f;
        
        /// <summary>
        /// The amount the player must drag in the y-axis to detach a brick if it
        /// could be detached in either direction.
        /// </summary>
        private const float YDragDetachThreshold = 12f;
        
        
        /// <summary>
        /// The time period to not bother updating the player input data.
        /// </summary>
        private static readonly TimeSpan CacheTime =
            TimeSpan.FromMilliseconds(ApproxOneTick);
        
        /// <summary>
        /// The size of the isometric view cast on objects in the scene that may
        /// overlap other objects on the grid.
        /// </summary>
        private static readonly Size IsometricViewOverlapping =
            new Size(11, 14);
        

        /// <summary>
        /// The bricks that are currently held in the player's hand.
        /// </summary>
        private List<BrickActor> Hand { get; set; }
        
        /// <summary>
        /// The offset of the bricks original location versus the location that the
        /// pick up occurred.
        /// </summary>
        private PointF HandPickupOffset { get; set; }
        
        /// <summary>
        /// The location that the pick up occurred.
        /// </summary>
        private PointF HandPickupPosition { get; set; }

        /// <summary>
        /// The collection of bricks that have been mapped as nodes.
        /// </summary>
        private IEnumerable<BrickActor> MappedBricks
        {
            get { return Nodes.Keys; }
        }
        
        /// <summary>
        /// The map of nodes representing bricks that can be picked up, containing
        /// information about their connections to other bricks/nodes.
        /// </summary>
        private Dictionary<BrickActor, BrickConnectionNode> Nodes { get; set; }
        
        /// <summary>
        /// The scene.
        /// </summary>
        private Scene Scene { get; set; }


        #region Player Input Related
        
        /// <summary>
        /// The value that indicates whether a mouse drag has been started.
        /// </summary>
        private bool Dragging { get; set; }

        /// <summary>
        /// The value that indicates the ability to place bricks based on the
        /// mouse position in the last input update.
        /// </summary>
        private bool LastCanPlace { get; set; }

        /// <summary>
        /// The last brick that the detach direction check was ran on.
        /// </summary>
        private BrickActor LastCheckedBrick { get; set; }

        /// <summary>
        /// The detach direction based on the mouse position in the last input
        /// update.
        /// </summary>
        private BrickDetachDirection LastDetachDirection { get; set; }
        
        /// <summary>
        /// The last position of the mouse.
        /// </summary>
        private PointF LastMousePosition { get; set; }

        /// <summary>
        /// The time of the last input update.
        /// </summary>
        private DateTime LastUpdateTime { get; set; }
        
        /// <summary>
        /// The mouse position at which dragging started when determining which
        /// direction to detach bricks.
        /// </summary>
        private PointF StartDrag { get; set; }

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="BrickPicker"/> class.
        /// </summary>
        /// <param name="scene">
        /// The scene.
        /// </param>
        public BrickPicker(
            Scene scene
        )
        {
            Hand           = new List<BrickActor>();
            LastUpdateTime = DateTime.MinValue;
            Nodes          = new Dictionary<BrickActor, BrickConnectionNode>();
            Scene          = scene;

            MapBrickConnections();
        }
        
        
        /// <summary>
        /// Determines whether the specified brick can be detached.
        /// </summary>
        /// <param name="brick">
        /// The brick.
        /// </param>
        /// <returns>
        /// True if the brick can be detached.
        /// </returns>
        public bool CanDetachBrick(
            BrickActor brick
        )
        {
            return WhatIfDetach(
                brick,
                BrickDetachDirection.Either,
                out IList<BrickActor> ignorePickups
            );
        }
        
        /// <summary>
        /// Determines whether the active hand of bricks can be placed starting at the
        /// specified location.
        /// </summary>
        /// <param name="location">
        /// The origin to begin placing bricks.
        /// </param>
        /// <returns>
        /// True if the bricks can be placed.
        /// </returns>
        public bool CanPlaceHand(
            PointF location
        )
        {
            if (Hand.Count == 0)
            {
                throw new InvalidOperationException(
                    "The player's hand is empty."
                );
            }

            Point movement  = Scene.PointToGrid(
                                  location.Subtract(HandPickupPosition).ToPoint()
                              );
            var   newDetach = BrickDetachDirection.Either;
            
            foreach (BrickActor brick in Hand)
            {
                Point newTarget = brick.Location.Add(movement);
                var   newRange  = new Rectangle(
                                      newTarget,
                                      brick.GridSize
                                  );
                
                // Check region itself is free (and within the play field)
                //
                if (!Scene.CheckGridRegionFreeForActor(brick, newRange))
                {
                    return false;
                }

                // Check connections...
                //
                var offset    = new Point(0, 1);
                var aboveRect = newRange.SubtractOffset(offset);
                var belowRect = newRange.AddOffset(offset);
                
                if (Scene.RegionContainsBricksOrNull(aboveRect))
                {
                    IList<BrickActor> bricks =
                        Scene.GetActorsInRegion<BrickActor>(aboveRect);
                    
                    if (bricks.Any())
                    {
                        // Check... have we already got a connection below?
                        //
                        if (newDetach == BrickDetachDirection.Upwards)
                        {
                            return false;
                        }

                        newDetach = BrickDetachDirection.Downwards;
                    }
                }
                
                if (Scene.RegionContainsBricksOrNull(belowRect))
                {
                    IList<BrickActor> bricks =
                        Scene.GetActorsInRegion<BrickActor>(belowRect);
                    
                    if (bricks.Any())
                    {
                        // Check... already have a connection above?
                        //
                        if (newDetach == BrickDetachDirection.Downwards)
                        {
                            return false;
                        }

                        newDetach = BrickDetachDirection.Upwards;
                    }
                }
            }
            
            // Check we have a connection at all
            //
            if (newDetach == BrickDetachDirection.Either)
            {
                return false;
            }

            return true; // All good!
        }

        /// <summary>
        /// Detaches bricks from the specified location.
        /// </summary>
        /// <param name="location">
        /// The location at which to detach bricks.
        /// </param>
        /// <param name="direction">
        /// The direction to try detaching bricks, pass
        /// <see cref="BrickDetachDirection.Either"/> if attempting to detach without
        /// having a direction in mind.
        /// </param>
        /// <returns>
        /// True if bricks were detached.
        /// </returns>
        public bool DetachAt(
            PointF               location,
            BrickDetachDirection direction = BrickDetachDirection.Either
        )
        {
            if (Hand.Count > 0)
            {
                throw new InvalidOperationException(
                    "The player's hand already contains bricks, they must be " +
                    "placed first."
                );
            }

            // Obtain a brick from the scene...
            //
            var brick = HitTest(location);
            
            if (brick == null)
            {
                throw new InvalidOperationException(
                    "There was no brick at the cell."
                );
            }

            // Now collect the bricks
            //
            IList<BrickActor> bricks    = null;
            bool              canDetach = WhatIfDetach(brick, direction, out bricks);
            
            if (!canDetach)
            {
                return false;
            }
            
            // Remove bricks and add to hand
            //
            foreach (BrickActor toPickup in bricks)
            {
                Scene.RemoveActor(toPickup);
                Hand.Add(toPickup);
            }

            Hand.Sort(new ActorZOrderComparer()); // Aid in rendering

            // Store the offset from where we picked up the brick vs. its actual
            // 'origin'
            //
            HandPickupOffset   = brick.Location.ToPointF().Subtract(location);
            HandPickupPosition = location;

            return true;
        }

        /// <summary>
        /// Gets the direction a brick can be detached towards.
        /// </summary>
        /// <param name="brick">
        /// The brick.
        /// </param>
        /// <returns>
        /// The direction in which the brick may be detached.
        /// </returns>
        public BrickDetachDirection GetDetachDirectionForBrick(
            BrickActor brick
        )
        {
            BrickConnectionNode node = Nodes[brick];
            
            switch (node.GreyBrickPath)
            {
                case GreyBrickPathState.EitherPath:
                case GreyBrickPathState.NoPath:
                    return BrickDetachDirection.Either;

                case GreyBrickPathState.PathDownwards:
                    return BrickDetachDirection.Upwards;

                case GreyBrickPathState.PathUpwards:
                    return BrickDetachDirection.Downwards;

                default:
                    throw new NotSupportedException(
                        "Unknown path state."
                    );
            }
        }

        /// <summary>
        /// Processes brick connections in the scene and constructs a fresh node map.
        /// </summary>
        public void MapBrickConnections()
        {
            IList<BrickActor> startingPoints = ConnectBricks();
            
            foreach (BrickActor brick in startingPoints)
            {
                RecursiveMarkGreyBrickPathDirection(
                    brick,
                    Nodes[brick].GreyBrickPath
                );
            }
        }
        
        /// <summary>
        /// Attempts to place bricks at the specified location.
        /// </summary>
        /// <param name="location">
        /// The origin to begin placing bricks.
        /// </param>
        /// <returns>
        /// True if the bricks were placed.
        /// </returns>
        public bool PlaceAt(
            PointF location
        )
        {
            if (!CanPlaceHand(location))
            {
                return false;
            }

            // Placement confirmed acceptable, so place the bricks in the scene now
            //
            Point movement =
                Scene.PointToGrid(
                    location.Subtract(HandPickupPosition).ToPoint()
                );
            
            foreach (BrickActor brick in Hand)
            {
                Point newTarget = brick.Location.Add(movement);

                Scene.AddActor(brick, newTarget);
            }

            Hand.Clear();
            MapBrickConnections();

            HandPickupOffset   = Point.Empty;
            HandPickupPosition = Point.Empty;

            return true;
        }
        
        /// <summary>
        /// Renders the player's hand.
        /// </summary>
        /// <param name="sb">
        /// The scene sprite batch.
        /// </param>
        public void RenderFrame(
            ISpriteBatch sb
        )
        {
            Point movement    = Scene.PointToGrid(
                                    LastMousePosition.Subtract(HandPickupPosition)
                                                     .ToPoint()
                                );
            float pickedAlpha = LastCanPlace ? 0.8f : 0.4f;
            
            foreach (BrickActor brick in Hand)
            {
                SpriteAnimationSpriteData spriteData =
                    brick.GetSpriteAtCell(brick.Location.X, brick.Location.Y);

                // Draw in the pos by the mouse
                //
                Point final = brick.Location.Add(movement)
                                            .Product(Scene.CellSize);

                sb.Draw(
                    sb.Atlas.Sprites[spriteData.SpriteName],
                    final,
                    Color.Transparent,
                    pickedAlpha
                );
            }
        }

        /// <summary>
        /// Updates the brick picker.
        /// </summary>
        /// <param name="game">
        /// The running Junkbot game instance.
        /// </param>
        /// <param name="deltaTime">
        /// The time difference since the last update.
        /// </param>
        /// <param name="inputs">
        /// The latest state of inputs.
        /// </param>
        public void Update(
            JunkbotGame game,
            TimeSpan    deltaTime,
            InputEvents inputs
        )
        {
            var mousePos = game.EngineHost.Renderer.PointToViewport(
                               inputs.MousePosition
                           );
            var now      = DateTime.UtcNow;

            // Hit test the brick first - due to the isometric view, we can't just
            // cast directly to a grid cell, we need to try a hitbox that matches the
            // 'real' size of the brick
            //
            var  brick                 = HitTest(mousePos);
            bool updateThresholdPassed = LastUpdateTime.Add(CacheTime) < now;
            
            if (brick?.Color == Color.Gray)
            {
                brick = null; // Ignore grey bricks
            }

            LastMousePosition = mousePos;
            
            // If we have a hand, check if we can place the hand, otherwise
            // check if we can pick up
            //
            if (Hand.Any())
            {
                if (
                    updateThresholdPassed ||
                    LastCheckedBrick != brick
                )
                {
                    LastCanPlace   = CanPlaceHand(mousePos);
                    LastUpdateTime = now;
                }
            }
            else
            {
                if (
                    updateThresholdPassed ||
                    LastCheckedBrick != brick
                )
                {
                    if (brick == null)
                    {
                        LastCheckedBrick    = null;
                        LastDetachDirection = BrickDetachDirection.Either;
                    }
                    else
                    {
                        LastCheckedBrick    = brick;
                        LastDetachDirection = GetDetachDirectionForBrick(brick);
                    }
                    
                    LastUpdateTime = now;
                }
            }
            
            // Handle brick detachment / placement
            //
            if (Hand.Any())
            {
                if (
                    LastCanPlace &&
                    inputs.NewPresses.Contains(ControlInput.MouseButtonLeft)
                )
                {
                    PlaceAt(mousePos);
                    LastCanPlace = false;
                }
            }
            else
            {
                var  detachDir    = BrickDetachDirection.Either;
                bool shouldDetach = false;

                if (inputs.NewPresses.Contains(ControlInput.MouseButtonLeft))
                {
                    if (LastCheckedBrick != null)
                    {
                        if (LastDetachDirection == BrickDetachDirection.Either)
                        {
                            Dragging  = true;
                            StartDrag = mousePos;
                        }
                        else
                        {
                            detachDir    = LastDetachDirection;
                            shouldDetach = true;
                        }
                    }
                }
                else if (
                    Dragging &&
                    inputs.DownedInputs.Contains(ControlInput.MouseButtonLeft)
                )
                {
                    float diff = StartDrag.Y - mousePos.Y;
                    
                    if (Math.Abs(diff) > YDragDetachThreshold)
                    {
                        if (diff > 0)
                        {
                            detachDir = BrickDetachDirection.Upwards;
                        }
                        else
                        {
                            detachDir = BrickDetachDirection.Downwards;
                        }

                        shouldDetach = true;
                    }
                }
                else if (inputs.NewReleases.Contains(ControlInput.MouseButtonLeft))
                {
                    Dragging  = false;
                    StartDrag = PointF.Empty;
                }
                
                if (shouldDetach)
                {
                    if (Dragging)
                    {
                        DetachAt(
                            StartDrag,
                            detachDir
                        );
                    }
                    else
                    {
                        DetachAt(
                            mousePos,
                            detachDir
                        );
                    }

                    Dragging            = false;
                    LastCheckedBrick    = null;
                    LastDetachDirection = BrickDetachDirection.Either;
                    StartDrag           = PointF.Empty;
                }
            }
        }

        /// <summary>
        /// Determines the bricks that would be picked up after trying to detach a
        /// specific brick.
        /// </summary>
        /// <param name="brick">
        /// The brick.
        /// </param>
        /// <param name="direction">
        /// The direction to try detaching bricks, pass
        /// <see cref="BrickDetachDirection.Either"/> if attempting to detach without
        /// having a direction in mind.
        /// </param>
        /// <param name="pickedUpBricks">
        /// (Output) The bricks that were detached.
        /// </param>
        /// <returns>
        /// True if either the bricks were picked up, or the brick needs a specific
        /// direction to be detached.
        /// </returns>
        public bool WhatIfDetach(
            BrickActor            brick,
            BrickDetachDirection  direction,
            out IList<BrickActor> pickedUpBricks
        )
        {
            if (brick.Color == Color.Gray)
            {
                throw new ArgumentException(
                    "Grey bricks cannot be detached."
                );
            }
            
            // We should have the brick
            //
            var rootNode = Nodes[brick];
            
            pickedUpBricks = null;
            
            if (direction == BrickDetachDirection.Either)
            {
                switch (rootNode.GreyBrickPath)
                {
                    case GreyBrickPathState.EitherPath:
                    case GreyBrickPathState.NoPath:
                        // Scan pickups in either direction - could be blocked either
                        // way
                        //
                        var downBricks = AttemptCollectBricks(
                                             brick,
                                             BrickDetachDirection.Downwards
                                         );
                        var upBricks   = AttemptCollectBricks(
                                             brick,
                                             BrickDetachDirection.Upwards
                                         );
                        
                        if (
                            downBricks.Count == 0 ||
                            upBricks.Count   == 0
                        )
                        {
                            return false; // Blocked!
                        }
                        
                        // We reached here - the bricks can be picked up however we
                        // need a specific direction
                        //
                        return true;
                    
                    case GreyBrickPathState.PathDownwards:
                        direction = BrickDetachDirection.Upwards;
                        break;
                    
                    case GreyBrickPathState.PathUpwards:
                        direction = BrickDetachDirection.Downwards;
                        break;
                }
            }
            
            // Check the bricks are not blocked
            //
            var bricks = AttemptCollectBricks(brick, direction);
            
            if (bricks.Count == 0)
            {
                return false; // Blocked!
            }
            
            // All good, we can pick up these bricks
            //
            pickedUpBricks = bricks;

            return true;
        }
        
        
        /// <summary>
        /// Attempts to collect bricks that will be detached given a starting point and
        /// direction
        /// </summary>
        /// <param name="brick">
        /// The brick from which to start collecting.
        /// </param>
        /// <param name="direction">
        /// The direction to collect bricks
        /// </param>
        /// <returns>
        /// The collected bricks as an <see cref="IList{T}"/> collection, if empty
        /// then the attempt was blocked by a mobile actor.
        /// </returns>
        private IList<BrickActor> AttemptCollectBricks(
            BrickActor            brick,
            BrickDetachDirection  direction
        )
        {
            bool wasAdded = false;
            
            // Collect all bricks in the direction first
            //
            var pickedBricks = new List<BrickActor>();
            
            wasAdded =
                RecursiveCollectBricks(
                    brick,
                    direction,
                    ref pickedBricks
                );
            
            if (!wasAdded)
            {
                return new List<BrickActor>().AsReadOnly();
            }

            // Now collect any bricks that have no other grey brick
            // attachments
            //
            // Of course we create a new list because pickedBricks
            // will be edited as we iterate
            //
            var ineligible     = new List<BrickActor>();
            var toInspect      = new List<BrickActor>(pickedBricks);
            var trackInspected = new List<BrickActor>();
            
            foreach (BrickActor nextBrick in toInspect)
            {
                wasAdded =
                    RecursiveCollectBricksAny(
                        nextBrick,
                        ref trackInspected,
                        ref pickedBricks,
                        ref ineligible,
                        out bool ignoreIneligible
                    );
                
                if (!wasAdded)
                {
                    return new List<BrickActor>().AsReadOnly();
                }
            }
            
            return pickedBricks.AsReadOnly();
        }

        /// <summary>
        /// Connects the bricks.
        /// </summary>
        /// <returns>
        /// The bricks that are connected directly to grey bricks as an
        /// <see cref="IList{T}"/> collection.
        /// </returns>
        private IList<BrickActor> ConnectBricks()
        {
            IList<BrickActor> bricks = Scene.GetActorsOfType<BrickActor>();
            var               starts = new List<BrickActor>();
            
            Nodes.Clear();
            
            foreach (BrickActor brick in bricks)
            {
                if (brick.Color == Color.Gray)
                {
                    continue;
                }
                
                var node = new BrickConnectionNode(Scene, brick);
                
                Nodes.Add(
                    brick,
                    node
                );
                
                if (node.GreyBrickPath != GreyBrickPathState.NoPath)
                {
                    starts.Add(brick);
                }
            }

            return starts.AsReadOnly();
        }
        
        /// <summary>
        /// Performs a hit test on bricks based on the viewport.
        /// </summary>
        /// <param name="pos">
        /// The position in the viewport to perform the hit test.
        /// </param>
        /// <returns>
        /// The brick that occupies the specified position, null if no bricks were
        /// found.
        /// </returns>
        private BrickActor HitTest(
            PointF pos
        )
        {
            var hitBricks = new List<BrickActor>();
            
            // Hit test each brick based on their size in the isometric view
            //
            foreach (BrickActor brick in MappedBricks)
            {
                Rectangle brickHitBox =
                    brick.BoundingBox.Scale(Scene.CellSize)
                                     .AddSize(IsometricViewOverlapping);

                if (
                    Collision.PointInRect(
                        pos,
                        brickHitBox
                    )
                )
                {
                    hitBricks.Add(brick);
                }
            }

            // We sort by z-order and return the top-most brick
            //
            hitBricks.Sort(new ActorZOrderComparer());

            return hitBricks.LastOrDefault();
        }

        /// <summary>
        /// Recursively collects bricks to pick up in the specified direction.
        /// </summary>
        /// <param name="nextBrick">
        /// The brick.
        /// </param>
        /// <param name="direction">
        /// The direction to collect bricks.
        /// </param>
        /// <param name="collected">
        /// The collected bricks overall.
        /// </param>
        /// <returns>
        /// True if the brick was not blocked by an actor.
        /// </returns>
        private bool RecursiveCollectBricks(
            BrickActor           nextBrick,
            BrickDetachDirection direction,
            ref List<BrickActor> collected
        )
        {
            BrickConnectionNode node        = Nodes[nextBrick];
            IList<BrickActor>   connections =
                direction == BrickDetachDirection.Downwards ?
                    node.ConnectedBricksUnderside :
                    node.ConnectedBricksTopside;

            // Check if the brick is blocked from pickup
            //
            var checkRegion =
                new Rectangle(
                    nextBrick.Location.X,
                    nextBrick.Location.Y - 1,
                    nextBrick.GridSize.Width,
                    nextBrick.GridSize.Height
                );
            
            if (!Scene.RegionContainsBricksOrNull(checkRegion))
            {
                return false;
            }

            // All good, now check connected bricks
            //
            foreach (BrickActor brick in connections)
            {
                if (collected.Contains(brick))
                {
                    continue;
                }
                
                bool wasAdded =
                    RecursiveCollectBricks(
                        brick,
                        direction,
                        ref collected
                    );
                
                if (!wasAdded)
                {
                    return false; // The brick was blocked, so we are blocked too!
                }
            }
            
            collected.Add(nextBrick);

            return true;
        }

        /// <summary>
        /// Recursively collects bricks to pick up in any direction.
        /// </summary>
        /// <param name="brick">
        /// The brick.
        /// </param>
        /// <param name="trackInspected">
        /// The bricks that have or are being inspected - tracked during the active
        /// recursive stack to prevent infinite loops of inspecting the same brick
        /// multiple times.
        /// </param>
        /// <param name="collected">
        /// The collected bricks overall.
        /// </param>
        /// <param name="ineligible">
        /// The ineligible bricks overall.
        /// </param>
        /// <param name="isIneligible">
        /// (Output) True if the brick was determined to be ineligible.
        /// </param>
        /// <returns>
        /// True if the brick was not blocked by an actor.
        /// </returns>
        private bool RecursiveCollectBricksAny(
            BrickActor           brick,
            ref List<BrickActor> trackInspected,
            ref List<BrickActor> collected,
            ref List<BrickActor> ineligible,
            out bool             isIneligible
        )
        {
            // Check if the brick is blocked from pickup
            //
            var checkRegion =
                new Rectangle(
                    brick.Location.X,
                    brick.Location.Y - 1,
                    brick.GridSize.Width,
                    brick.GridSize.Height
                );
            
            if (!Scene.RegionContainsBricksOrNull(checkRegion))
            {
                isIneligible = false;
                return false;
            }
            
            // Check we haven't already got this brick in our tracking list, if we do
            // then we need to skip otherwise we'll enter an infinite loop
            //
            if (trackInspected.Contains(brick))
            {
                isIneligible = ineligible.Contains(brick);
                
                return true;
            }
            else
            {
                trackInspected.Add(brick);
            }
            
            // Now inspect child bricks (and collect this one if need be)
            //
            var node      = Nodes[brick];
            var toInspect = new List<BrickActor>();

            toInspect.AddRange(
                node.ConnectedBricksTopside
            );
            toInspect.AddRange(
                node.ConnectedBricksUnderside
            );
            
            isIneligible = false;
            
            if (collected.Contains(brick))
            {
                // The brick is already collected, so no need to determine if it's
                // blocked, just collect other bricks
                //
                foreach (BrickActor nextBrick in toInspect)
                {
                    if (
                        collected.Contains(nextBrick) ||
                        ineligible.Contains(nextBrick)
                    )
                    {
                        continue;
                    }
                    
                    var nextNode = Nodes[nextBrick];
                    
                    // If the next brick has a connection path to a grey brick, then
                    // it's not eligible
                    //
                    if (nextNode.GreyBrickPath != GreyBrickPathState.NoPath)
                    {
                        ineligible.Add(nextBrick);
                        continue;
                    }

                    bool wasAdded =
                        RecursiveCollectBricksAny(
                            nextBrick,
                            ref trackInspected,
                            ref collected,
                            ref ineligible,
                            out bool ignoreIneligible
                        );
                    
                    if (!wasAdded)
                    {
                        return false; // We are blocked
                    }
                }
            }
            else
            {
                foreach (BrickActor nextBrick in toInspect)
                {
                    if (collected.Contains(nextBrick))
                    {
                        continue;
                    }

                    bool nextIneligible = false;
                    var  nextNode       = Nodes[nextBrick];
                    
                    if (
                        ineligible.Contains(nextBrick) ||
                        nextNode.GreyBrickPath != GreyBrickPathState.NoPath
                    )
                    {
                        if (!ineligible.Contains(brick))
                        {
                            ineligible.Add(brick);
                        }
                        
                        isIneligible = true;

                        return true; // Wasn't eligible anyway so blocking doesn't
                                     // matter
                    }

                    bool wasAdded =
                        RecursiveCollectBricksAny(
                            nextBrick,
                            ref trackInspected,
                            ref collected,
                            ref ineligible,
                            out nextIneligible
                        );
                    
                    if (!wasAdded)
                    {
                        return false;
                    }

                    if (nextIneligible)
                    {
                        ineligible.Add(brick);
                        isIneligible = true;
                        
                        return true; // Ditto above - the connected brick is 
                                     // ineligible which means so is this one
                    }
                }
                
                collected.Add(brick);
            }
            
            return true;
        }
        
        /// <summary>
        /// Recursively marks the grey brick path.
        /// </summary>
        /// <param name="brick">
        /// The brick.
        /// </param>
        /// <param name="pathDirection">
        /// The direction of the path to a grey brick.
        /// </param>
        private void RecursiveMarkGreyBrickPathDirection(
            BrickActor         brick,
            GreyBrickPathState pathDirection
        )
        {
            BrickConnectionNode node = Nodes[brick];
            
            // Mark the child nodes in the direction
            //
            IList<BrickActor> toMark;
            
            switch (pathDirection)
            {
                case GreyBrickPathState.PathDownwards:
                    toMark = node.ConnectedBricksTopside;
                    break;
                
                case GreyBrickPathState.PathUpwards:
                    toMark = node.ConnectedBricksUnderside;
                    break;
                
                default:
                    throw new ArgumentException(
                        "The grey brick path state specified makes no sense."
                    );
            }
            
            foreach (BrickActor brickToMark in toMark)
            {
                RecursiveMarkGreyBrickPathDirection(brickToMark, pathDirection);
            }

            // Set state of this node
            //
            if (node.GreyBrickPath == GreyBrickPathState.NoPath)
            {
                node.GreyBrickPath = pathDirection;
            }
            else if (node.GreyBrickPath != pathDirection)
            {
                node.GreyBrickPath = GreyBrickPathState.EitherPath;
            }
        }
    }
}