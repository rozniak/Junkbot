/**
 * BrickMapNodeTestLongBridge.cs - Junkbot Brick Map Test (Nodes - Long Bridge)
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
    /// Tests brick picking logic for the long bridge case.
    /// </summary>
    [TestFixture]
    public class BrickPickerTestLongBridge : BrickPickerTestBase
    {
        [SetUp()]
        public void SetUp()
        {
            Cases =
                new List<BrickPickerTestCase>()
                {
                    // Brick 1 - Always down, pick up self and brick 2
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(12, 16),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(12, 16),
                                             new Point(14, 17)
                                         }
                    },
                    
                    // Brick 2 - Always down, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(14, 17),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(14, 17)
                                         }
                    },
                    
                    // Brick 3 - Either, we should need to clarify direction
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt  = new Point(15, 16),
                        Direction = BrickDetachDirection.Either
                    },
                    
                    // Brick 3 - Down, pick up self and bricks 2 and 4
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(15, 16),
                        Direction      = BrickDetachDirection.Downwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(14, 17),
                                             new Point(15, 16),
                                             new Point(16, 17)
                                         }
                    },
                    
                    // Brick 3 - Up, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(15, 16),
                        Direction      = BrickDetachDirection.Upwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(15, 16)
                                         }
                    },
                    
                    // Brick 4 - Either, we should need to clarify direction
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt  = new Point(16, 17),
                        Direction = BrickDetachDirection.Either
                    },
                    
                    // Brick 4 - Down, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(16, 17),
                        Direction      = BrickDetachDirection.Downwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(16, 17)
                                         }
                    },
                    
                    // Brick 4 - Up, pick up self and bricks 3 and 5
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(16, 17),
                        Direction      = BrickDetachDirection.Upwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(15, 16),
                                             new Point(16, 17),
                                             new Point(17, 16)
                                         }
                    },
                    
                    // Brick 5 - Either, we should need to clarify direction
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(17, 16),
                        Direction      = BrickDetachDirection.Either
                    },
                    
                    // Brick 5 - Down, pick up self and bricks 4 and 6
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(17, 16),
                        Direction      = BrickDetachDirection.Downwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(16, 17),
                                             new Point(17, 16),
                                             new Point(19, 17)
                                         }
                    },
                    
                    // Brick 6 - Always down, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(19, 17),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(19, 17)
                                         }
                    },
                    
                    // Brick 7 - Always down, pick up self and brick 6
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(21, 16),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(21, 16),
                                             new Point(19, 17)
                                         }
                    }
                };
            
            
            GameScene =
                Scene.FromLevel(
                    TestLevels.GetLevelPath("longbridge")
                );
        }
    }
}
