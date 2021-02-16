/**
 * BrickerPickerTestStairs.cs - Junkbot Brick Picking Test (Nodes - Simple Bridge)
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
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Tests.BrickPicking
{
    /// <summary>
    /// Tests brick picking logic for the simple stairs case.
    /// </summary>
    [TestFixture]
    public class BrickPickerTestStairs : BrickPickerTestBase
    {
        [SetUp()]
        public void SetUp()
        {
            Cases =
                new List<BrickPickerTestCase>()
                {
                    // Brick 1 - Always down, pick up all bricks
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(20, 19),
                        Direction      = BrickDetachDirection.Downwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(5,  12),
                                             new Point(7,  13),
                                             new Point(9,  14),
                                             new Point(12, 15),
                                             new Point(14, 16),
                                             new Point(16, 17),
                                             new Point(17, 18),
                                             new Point(20, 19)
                                         }
                    },
                    
                    // Brick 5 - Either, we should need to clarify direction
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt  = new Point(12, 15),
                        Direction = BrickDetachDirection.Either
                    },
                    
                    // Brick 5 - Down, pick up all bricks
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(12, 15),
                        Direction      = BrickDetachDirection.Downwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(5,  12),
                                             new Point(7,  13),
                                             new Point(9,  14),
                                             new Point(12, 15),
                                             new Point(14, 16),
                                             new Point(16, 17),
                                             new Point(17, 18),
                                             new Point(20, 19)
                                         }
                    },
                    
                    // Brick 5 - Up, pick up self and bricks 6, 7 and 8
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(12, 15),
                        Direction      = BrickDetachDirection.Upwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(5,  12),
                                             new Point(7,  13),
                                             new Point(9,  14),
                                             new Point(12, 15)
                                         }
                    }
                };

            GameScene =
                Scene.FromLevel(
                    TestLevels.GetLevelPath("stairs")
                );
        }
    }
}
