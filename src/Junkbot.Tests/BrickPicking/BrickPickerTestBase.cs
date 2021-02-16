/**
 * BrickPickerTestBase.cs - Junkbot Brick Picking Test Base Class
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game;
using Junkbot.Game.World.Actors;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Junkbot.Tests.BrickPicking
{
    /// <summary>
    /// Base class for the brick picking tests.
    /// </summary>
    [TestFixture]
    public abstract class BrickPickerTestBase
    {
        /// <summary>
        /// The test cases of bricks to pick up, and the bricks that should have been
        /// picked up in the case.
        /// </summary>
        protected List<BrickPickerTestCase> Cases { get; set; }
        
        /// <summary>
        /// The game scene.
        /// </summary>
        protected Scene GameScene { get; set; }
        
        
        [Test()]
        public void TestCase()
        {
            for (int i = 0; i < Cases.Count; i++)
            {
                var testCase = Cases[i];
                var expected = new List<BrickActor>();
                int pickupX  = testCase.DetachAt.X;
                int pickupY  = testCase.DetachAt.Y;
                
                // Build up the bricks that we expect to be picked up
                //
                foreach (Point loc in testCase.ExpectedBricks)
                {
                    expected.Add(
                        GameScene.GetActorAtCell<BrickActor>(
                            loc.X,
                            loc.Y
                        )
                    );
                }

                // Pick up now
                //
                IList<BrickActor> picked;
                bool              res =
                    GameScene.BrickPicker.WhatIfDetach(
                        GameScene.GetActorAtCell<BrickActor>(pickupX, pickupY),
                        testCase.Direction,
                        out picked
                    );
                
                // Assert the results
                //
                if (!testCase.ShouldBeBlocked)
                {
                    Assert.IsTrue(
                        res,
                        $"Case {i} - " +
                        "The pickup attempt was blocked when it shouldn't have been."
                    );
                    
                    // If we expected nothing returned, then the assumption is the
                    // brick picker needs clarification on the direction to pick up
                    //
                    if (expected.Any())
                    {
                        foreach (BrickActor brick in picked)
                        {
                            Assert.IsTrue(
                                expected.Remove(brick),
                                $"Case {i} - " +
                                "A brick was picked up that was unexpected."
                            );
                        }
                        
                        // If the lists were equal then there should be none left in
                        // the expected list now
                        //
                        Assert.IsFalse(
                            expected.Any(),
                            $"Case {i} - " +
                            "Not all the expected bricks were picked up."
                        );
                    }
                    else
                    {
                        Assert.IsNull(
                            picked,
                            $"Case {i} - " +
                            "Stuff was picked up when there shouldn't have been."
                        );
                    }
                }
                else
                {
                    Assert.IsFalse(
                        res,
                        $"Case {i} - " +
                        "The pickup attempt should've been blocked, but it wasn't."
                    );
                }
            }
        }
    }
}
