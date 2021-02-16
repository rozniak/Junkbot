/**
 * BrickMapNodeTestLedge.cs - Junkbot Brick Map Test (Nodes - Ledge)
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
    /// Tests brick picking logic for the ledge case.
    /// </summary>
    [TestFixture]
    public class BrickPickerTestLedge : BrickPickerTestBase
    {
        [SetUp()]
        public void SetUp()
        {
            Cases =
                new List<BrickPickerTestCase>()
                {
                    // Brick 1 - Blocked
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt        = new Point(19, 16),
                        Direction       = BrickDetachDirection.Either,
                        ShouldBeBlocked = true
                    },
                    
                    // Brick 2 - Blocked
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt        = new Point(18, 17),
                        Direction       = BrickDetachDirection.Either,
                        ShouldBeBlocked = true
                    },
                    
                    // Brick 3 - Always down, pick up self
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt       = new Point(19, 18),
                        Direction      = BrickDetachDirection.Either,
                        ExpectedBricks = new List<Point>()
                                         {
                                             new Point(19, 18)
                                         }
                    },
                    
                    // Brick 4 - Blocked
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt        = new Point(11, 18),
                        Direction       = BrickDetachDirection.Either,
                        ShouldBeBlocked = true
                    },
                    
                    // Brick 5 - Blocked
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt        = new Point(14, 17),
                        Direction       = BrickDetachDirection.Either,
                        ShouldBeBlocked = true
                    },
                    
                    // Brick 6 - Blocked
                    //
                    new BrickPickerTestCase()
                    {
                        DetachAt        = new Point(13, 16),
                        Direction       = BrickDetachDirection.Either,
                        ShouldBeBlocked = true
                    }
                };
            
            GameScene =
                Scene.FromLevel(
                    TestLevels.GetLevelPath("ledge")
                );
        }
    }
}
