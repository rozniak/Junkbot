/**
 * BrickPickerTestBridge.cs - Junkbot Brick Picking Test (Nodes - Simple Bridge)
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
    /// Tests brick picking logic for the simple bridge case.
    /// </summary>
    [TestFixture]
    public class BrickPickerTestBridge : BrickPickerTestBase
    {
        [SetUp()]
        public void SetUp()
        {
            Cases =
                new List<BrickPickerTestCase>()
                {
                    // Brick 1 - Always down, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(10, 15),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(10, 15)
                                         }
                    },
                    
                    // Brick 2 - Either, we should need to clarify direction
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt  = new Point(17, 14),
                        Direction = BrickDetachDirection.Either
                    },
                    
                    // Brick 2 - Down, pick up all bricks
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(17, 14),
                        Direction      = BrickDetachDirection.Downwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(10, 15),
                                             new Point(17, 14),
                                             new Point(18, 15)
                                         }
                    },
                    
                    // Brick 2 - Up, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(17, 14),
                        Direction      = BrickDetachDirection.Upwards,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(17, 14)
                                         }
                    },
                    
                    // Brick 3 - Always down, picks up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(18, 15),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(18, 15)
                                         }
                    }
                };
            
            GameScene =
                new Scene(new JunkbotLevel(TestLevels.GetLevelPath("bridge")));
        }
    }
}
