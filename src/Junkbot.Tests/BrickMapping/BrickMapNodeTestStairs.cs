/**
 * BrickMapNodeTestStairs.cs - Junkbot Brick Map Test (Nodes - Simple Stairs)
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
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Tests.BrickMapping
{
    /// <summary>
    /// Tests brick picker node algorithms for the simple stairs case.
    /// </summary>
    [TestFixture]
    public class BrickMapNodeTestStairs : BrickMapNodeTestBase
    {
        [SetUp()]
        public void SetUp()
        {
            Cases =
                new List<Tuple<Point, BrickDetachDirection>>()
                {
                    // Brick 1 - Down
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(20, 19),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 8 - Either
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(5, 12),
                        BrickDetachDirection.Either
                    )
                };

            GameScene =
                new Scene(new JunkbotLevel(TestLevels.GetLevelPath("stairs")));
        }
    }
}
