/**
 * BrickPlacementTestBase.cs - Junkbot Brick Placing Test Base Class
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game;
using Junkbot.Game.World.Actors;
using NUnit.Framework;
using Oddmatics.Rzxe.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Junkbot.Tests.BrickPlacing
{
    /// <summary>
    /// Base class for the brick placing tests.
    /// </summary>
    [TestFixture]
    public abstract class BrickPlacementTestBase
    {
        /// <summary>
        /// The test cases of bricks to pick up and place down - where they should end
        /// up.
        /// </summary>
        protected List<BrickPlacementTestCase> Cases { get; set; }
        
        /// <summary>
        /// The game scene.
        /// </summary>
        protected Scene GameScene { get; set; }
        
        
        [Test()]
        public void TestCase()
        {
            for (int i = 0; i < Cases.Count; i++)
            {
                BrickPlacementTestCase testCase = Cases[i];

                // Retrieve the bricks at their starting positions
                //
                var starts = new Dictionary<Point, BrickActor>();
                
                foreach (Tuple<Point, Point> datum in testCase.ExpectedPlacements)
                {
                    Point      originalPosition = datum.Item2;
                    BrickActor theActor =
                        GameScene.GetActorAtCell<BrickActor>(
                            originalPosition.X,
                            originalPosition.Y
                        );

                    Assert.IsNotNull(
                        theActor,
                        $"Case {i} - " +
                        $"Unable to locate brick at {originalPosition}."
                    );

                    starts.Add(
                        originalPosition,
                        theActor
                    );
                }

                // If nothing expected, then assume the attempt should be blocked
                //
                bool shouldBeBlocked = !starts.Any();

                // Do the placement stuff - scale up and shift the location from the
                // grid so it passes the hit test
                //
                Point yOffset        = new Point(0, GameScene.CellSize.Height);
                Point scaledDetachAt = testCase.DetachAt.Product(GameScene.CellSize)
                                                        .Add(yOffset);
                Point scaledPlaceAt  = testCase.PlaceAt.Product(GameScene.CellSize)
                                                       .Add(yOffset);
                
                GameScene.BrickPicker.DetachAt(
                    scaledDetachAt,
                    testCase.Direction
                );

                bool successfulPlacement =
                    GameScene.BrickPicker.PlaceAt(scaledPlaceAt.ToPointF());
                
                // Now check the results
                //
                if (shouldBeBlocked)
                {
                    Assert.IsFalse(
                        successfulPlacement,
                        $"Case {i} - " +
                        "The placement attempt should've been blocked but it wasn't."
                    );

                    // Put it back where it came from so we can continue tests
                    //
                    Assert.IsTrue(
                        GameScene.BrickPicker.PlaceAt(
                            scaledDetachAt
                        ),
                        $"Case {i} - " +
                        "Tried to put bricks back but the attempt was blocked."
                    );

                    continue; // Nothing more required, we're done here!
                }

                Assert.IsTrue(
                    successfulPlacement,
                    $"Case {i} - " +
                    "The placement attempt was blocked when it shouldn't have been."
                );
                
                foreach (Tuple<Point, Point> datum in testCase.ExpectedPlacements)
                {
                    Point      newPosition      = datum.Item1;
                    Point      originalPosition = datum.Item2;
                    BrickActor originalActor    = starts[originalPosition];
                    BrickActor testActor        =
                        GameScene.GetActorAtCell<BrickActor>(
                            newPosition.X,
                            newPosition.Y
                        );

                    Assert.IsNotNull(
                        testActor,
                        $"Case {i} - " +
                        $"There should've been a brick at {newPosition}, but there " +
                        "wasn't."
                    );
                    
                    Assert.AreSame(
                        originalActor,
                        testActor,
                        $"Case {i} - " +
                        $"The brick placed at {newPosition} doesn't match the one " +
                        $"that used to be at {originalPosition}."
                    );
                }
            }
        }
    }
}
