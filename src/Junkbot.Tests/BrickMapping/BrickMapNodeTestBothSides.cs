/**
 * BrickMapNodeTestBothSides.cs - Junkbot Brick Map Test (Nodes - Complex Both Sides)
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
    /// Tests brick picker node algorithms for the complex connections, bricks
    /// connected on both sides (top and bottom) case.
    /// </summary>
    [TestFixture]
    public class BrickMapNodeTestBothSides : BrickMapNodeTestBase
    {
        [SetUp()]
        public void SetUp()
        {
            Cases =
                new List<Tuple<Point, BrickDetachDirection>>()
                {
                    // Brick 1 - Up
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(10, 15),
                        BrickDetachDirection.Upwards
                    ),
                    
                    // Brick 2 - Either
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(11, 16),
                        BrickDetachDirection.Either
                    ),
                    
                    // Brick 3 - Either
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(14, 17),
                        BrickDetachDirection.Either
                    ),
                    
                    // Brick 4 - Up
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(19, 15),
                        BrickDetachDirection.Upwards
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
                        new Point(15, 18),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 8 - Down
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(7, 18),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 9 - Down
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(6, 17),
                        BrickDetachDirection.Downwards
                    )
                };


            GameScene =
                new Scene(new JunkbotLevel(TestLevels.GetLevelPath("bothsides")));
        }
    }
}
