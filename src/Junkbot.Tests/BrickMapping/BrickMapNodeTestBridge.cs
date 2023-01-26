/**
 * BrickMapNodeTestBridge.cs - Junkbot Brick Map Test (Nodes - Simple Bridge)
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
    /// Tests brick picker node algorithms for the simple bridge case.
    /// </summary>
    [TestFixture()]
    public class BrickMapNodeTestBridge : BrickMapNodeTestBase
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
                        new Point(10, 15),
                        BrickDetachDirection.Downwards
                    ),
                    
                    // Brick 2 - Either
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(17 ,14),
                        BrickDetachDirection.Either
                    ),
                    
                    // Brick 3 - Down
                    //
                    new Tuple<Point, BrickDetachDirection>(
                        new Point(18, 15),
                        BrickDetachDirection.Downwards
                    )
                };


            GameScene =
                new Scene(new JunkbotLevel(TestLevels.GetLevelPath("bridge")));
        }
    }
}
