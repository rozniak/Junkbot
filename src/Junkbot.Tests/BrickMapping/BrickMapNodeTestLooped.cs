/**
 * BrickMapNodeTestLooped.cs - Junkbot Brick Map Test (Nodes - Looped)
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
    /// Tests brick picker node algorithms for the looped connections case.
    /// </summary>
    [TestFixture]
    public class BrickMapNodeTestLooped : BrickMapNodeTestBase
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
                        new Point(14, 18),
                        BrickDetachDirection.Upwards
                    ),
                    
                    // Brick 2 - Up
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(13, 17),
                        BrickDetachDirection.Upwards
                    ),
                    
                    // Brick 3 - Up
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(17, 17),
                        BrickDetachDirection.Upwards
                    ),
                    
                    // Brick 4 - Up
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(14, 16),
                        BrickDetachDirection.Upwards
                    ),
                    
                    // Brick 5 - Up
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(11, 15),
                        BrickDetachDirection.Upwards
                    ),
                    
                    // Brick 6 - Up
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(15, 15),
                        BrickDetachDirection.Upwards
                    ),
                    
                    // Brick 7 - Up
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(17, 15),
                        BrickDetachDirection.Upwards
                    ),
                    
                    // Brick 8 - Up
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(13, 14),
                        BrickDetachDirection.Upwards
                    ),
                    
                    // Brick 9 - Up
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(16, 14),
                        BrickDetachDirection.Upwards
                    )
                };
            
            GameScene =
                new Scene(new JunkbotLevel(TestLevels.GetLevelPath("looped")));
        }
    }
}
