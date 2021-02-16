/**
 * BrickPlacementTestBridge.cs - Junkbot Brick Placing Test (Nodes - Simple Bridge)
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game;
using Junkbot.Game.World.Logic;
using Junkbot.Tests.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Tests.BrickPlacing
{
    [TestFixture]
    public class BrickPlacementTestBridge : BrickPlacementTestBase
    {
        [SetUp()]
        public void SetUp()
        {
            Cases =
                new List<BrickPlacementTestCase>()
                {
                    // Brick 1 - Try placing overlapping another brick exactly
                    // (blocked)
                    //
                    new BrickPlacementTestCase()
                    {
                        DetachAt           = new Point(10, 15),
                        Direction          = BrickDetachDirection.Downwards,
                        ExpectedPlacements = new List<Tuple<Point, Point>>(),
                        PlaceAt            = new Point(18, 15)
                    },
                    
                    // Brick 2 - Try placing overlapping another brick via offset
                    // (blocked)
                    //
                    new BrickPlacementTestCase()
                    {
                        DetachAt           = new Point(19, 14),
                        Direction          = BrickDetachDirection.Upwards,
                        ExpectedPlacements = new List<Tuple<Point, Point>>(),
                        PlaceAt            = new Point(22, 15)
                    },
                    
                    // Brick 2 - Try placing with no connections
                    // (blocked)
                    //
                    new BrickPlacementTestCase()
                    {
                        DetachAt           = new Point(17, 14),
                        Direction          = BrickDetachDirection.Upwards,
                        ExpectedPlacements = new List<Tuple<Point, Point>>(),
                        PlaceAt            = new Point(17, 10)
                    },
                    
                    // Brick 2 - Try placing under bricks 1 and 3
                    //
                    new BrickPlacementTestCase()
                    {
                        DetachAt           = new Point(17, 14),
                        Direction          = BrickDetachDirection.Upwards,
                        ExpectedPlacements = new List<Tuple<Point, Point>>()
                                             {
                                                 new Tuple<Point, Point>(
                                                     new Point(17, 16),
                                                     new Point(17, 14)
                                                 )
                                             },
                        PlaceAt            = new Point(17, 16)
                    },
                    
                    // Brick 3 - Try picking up (taking 2 and 3) place on brick 1
                    //
                    new BrickPlacementTestCase()
                    {
                        DetachAt           = new Point(19, 15),
                        Direction          = BrickDetachDirection.Downwards,
                        ExpectedPlacements = new List<Tuple<Point, Point>>()
                                             {
                                                 new Tuple<Point, Point>(
                                                     new Point(18, 13),
                                                     new Point(18, 15)
                                                 ),
                                                 new Tuple<Point, Point>(
                                                     new Point(17, 14),
                                                     new Point(17, 16)
                                                 )
                                             },
                        PlaceAt            = new Point(19, 13)
                    }
                };

            GameScene =
                Scene.FromLevel(
                    TestLevels.GetLevelPath("bridge")
                );
        }
    }
}
