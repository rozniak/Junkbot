/**
 * BrickMapNodeTestLedge.cs - Junkbot Brick Map Test (Nodes - Ledge)
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
    /// Tests brick picker node algorithms for the ledge case.
    /// </summary>
    [TestFixture]
    public class BrickMapNodeTestLedge : BrickMapNodeTestBase
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
                        new Point(19, 16),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 2 - Down
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(18, 17),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 3 - Down
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(19, 18),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 4 - Down
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(11, 18),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 5 - Either
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(14, 17),
                        BrickDetachDirection.Either
                    ),
                    
                    // Brick 6 - Either
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(13, 16),
                        BrickDetachDirection.Either
                    )
                };

            GameScene =
                new Scene(new JunkbotLevel(TestLevels.GetLevelPath("ledge")));
        }
    }
}
