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
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Tests.BrickMapping
{
    /// <summary>
    /// Tests brick picker node algorithms for the long bridge case.
    /// </summary>
    [TestFixture]
    public class BrickMapNodeTestLongBridge : BrickMapNodeTestBase
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
                        new Point(13, 16),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 2 - Down
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(14, 17),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 3 - Either
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(15, 16),
                        BrickDetachDirection.Either
                    ),
                    
                    // Brick 4 - Either
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(16, 17),
                        BrickDetachDirection.Either
                    ),
                    
                    // Brick 5 - Either
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(17, 16),
                        BrickDetachDirection.Either
                    ),
                    
                    // Brick 6 - Down
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(19, 17),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 7 - Down
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(21, 16),
                        BrickDetachDirection.Downwards
                    )
                };

            GameScene =
                Scene.FromLevel(
                    TestLevels.GetLevelPath("longbridge")
                );
        }
    }
}
