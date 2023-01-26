/**
 * BrickerPickerTestBothSides.cs - Junkbot Brick Picking Test
 *                                                        (Nodes - Complex Both Sides)
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game;
using Junkbot.Game.World.Level;
using Junkbot.Game.World.Logic;
using Junkbot.Tests.Util;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Tests.BrickPicking
{
    /// <summary>
    /// Tests brick picking logic for the complex connections, bricks connection on
    /// both sides (top and bottom) case.
    /// </summary>
    [TestFixture]
    public class BrickPickerTestBothSides : BrickPickerTestBase
    {
        [SetUp()]
        public void SetUp()
        {
            Cases =
                new List<BrickPickerTestCase>()
                {
                    // Brick 1 - Always up, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(10, 15),
                        Direction      = BrickDetachDirection.Upwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(10, 15)
                                         }
                    },
                    
                    // Brick 2 - Either, we should need to clarify direction
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt  = new Point(11, 16),
                        Direction = BrickDetachDirection.Either
                    },
                    
                    // Brick 2 - Down, pick up self and bricks 3, 7 and 8
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(11, 16),
                        Direction      = BrickDetachDirection.Downwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(11, 16),
                                             new Point(14, 17),
                                             new Point(15, 18),
                                             new Point(7,  18)
                                         }
                    },
                    
                    // Brick 2 - Up, pick up self and brick above
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(11, 16),
                        Direction      = BrickDetachDirection.Upwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(10, 15),
                                             new Point(11, 16)
                                         }
                    },
                    
                    // Brick 3 - Either, we should need to clarify direction
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(14, 17),
                        Direction      = BrickDetachDirection.Either
                    },
                    
                    // Brick 3 - Down, pick up self and bricks 7 and 8
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(14, 17),
                        Direction      = BrickDetachDirection.Downwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(14, 17),
                                             new Point(15, 18),
                                             new Point(7,  18)
                                         }
                    },
                    
                    // Brick 3 - Up, pick up self 
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(14, 17),
                        Direction      = BrickDetachDirection.Upwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(14, 17),
                                             new Point(11, 16),
                                             new Point(10, 15)
                                         }
                    },
                    
                    // Brick 4 - Always up, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(19, 15),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(19, 15)
                                         }
                    },
                    
                    // Brick 5 - Either, we should need to clarity direction
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(17, 16),
                        Direction      = BrickDetachDirection.Either
                    },
                    
                    // Brick 5 - Down, pick up self and bricks 6 and 7
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(17, 16),
                        Direction      = BrickDetachDirection.Downwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(17, 16),
                                             new Point(19, 17),
                                             new Point(15, 18)
                                         }
                    },
                    
                    // Brick 5 - Up, pick up self and brick 4
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(17, 16),
                        Direction      = BrickDetachDirection.Upwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(17, 16),
                                             new Point(19, 15)
                                         }
                    },
                    
                    // Brick 6 - Always down, pick up self and brick 7 
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(19, 17),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(19, 17),
                                             new Point(15, 18)
                                         }
                    },
                    
                    // Brick 7 - Always down, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(15, 18),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(15, 18)
                                         }
                    },
                    
                    // Brick 8 - Always down, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(7, 18),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(7, 18)
                                         }
                    },
                    
                    // Brick 9 - Always down, pick up self and brick 8
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(6, 17),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(6, 17),
                                             new Point(7, 18)
                                         }
                    }
                };

            GameScene =
                new Scene(new JunkbotLevel(TestLevels.GetLevelPath("bothsides")));
        }
    }
}
