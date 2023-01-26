/**
 * BrickerPickerTestLooped.cs - Junkbot Brick Picking Test (Nodes - Looped)
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
    /// Tests brick picking logic for the looped connections case.
    /// </summary>
    public class BrickPickerTestLooped : BrickPickerTestBase
    {
        [SetUp()]
        public void SetUp()
        {
            Cases =
                new List<BrickPickerTestCase>()
                {
                    // Brick 1 - Always up, pick up all bricks
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(14, 18),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(14, 18),
                                             new Point(13, 17),
                                             new Point(17, 17),
                                             new Point(14, 16),
                                             new Point(11, 15),
                                             new Point(15, 15),
                                             new Point(17, 15),
                                             new Point(13, 14),
                                             new Point(16, 14)
                                         }
                    },
                    
                    // Brick 2 - Always up, pick up self and bricks 4, 5, 6, 7, 8 and
                    //           9
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(13, 17),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(13, 17),
                                             new Point(14, 16),
                                             new Point(11, 15),
                                             new Point(15, 15),
                                             new Point(17, 15),
                                             new Point(13, 14),
                                             new Point(16, 14)
                                         }
                    },
                    
                    // Brick 3 - Always up, pick up self and bricks 4, 5, 6, 7, 8 and
                    //           9
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(17, 17),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(17, 17),
                                             new Point(14, 16),
                                             new Point(11, 15),
                                             new Point(15, 15),
                                             new Point(17, 15),
                                             new Point(13, 14),
                                             new Point(16, 14)
                                         }
                    },
                    
                    // Brick 4 - Always up, picks up self and bricks 5, 6, 7, 8 and 9
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(14, 16),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(14, 16),
                                             new Point(11, 15),
                                             new Point(15, 15),
                                             new Point(17, 15),
                                             new Point(13, 14),
                                             new Point(16, 14)
                                         }
                    },
                    
                    // Brick 5 - Always up, picks up self and brick 8
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(11, 15),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(11, 15),
                                             new Point(13, 14)
                                         }
                    },
                    
                    // Brick 6 - Always up, picks up self and bricks 8 and 9
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(15, 15),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(15, 15),
                                             new Point(13, 14),
                                             new Point(16, 14)
                                         }
                    },
                    
                    // Brick 7 - Always up, picks up self and bricks 9
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(17, 15),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(17, 15),
                                             new Point(16, 14)
                                         }
                    },
                    
                    // Brick 8 - Always up, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(13, 14),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(13, 14)
                                         }
                    },
                    
                    // Brick 9 - Always up, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(16, 14),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(16, 14)
                                         }
                    }
                };
            
            GameScene =
                new Scene(new JunkbotLevel(TestLevels.GetLevelPath("looped")));
        }
    }
}
