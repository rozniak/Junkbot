/**
 * BrickMapNodeTestBase.cs - Junkbot Brick Map Test Base Class
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game;
using Junkbot.Game.World.Actors;
using Junkbot.Game.World.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Tests.BrickMapping
{
    /// <summary>
    /// Base class for the brick picker mapping algorithm tests.
    /// </summary>
    public abstract class BrickMapNodeTestBase
    {
        /// <summary>
        /// The test cases.
        /// </summary>
        protected List<Tuple<Point, BrickDetachDirection>> Cases { get; set; }
        
        /// <summary>
        /// The game scene.
        /// </summary>
        protected Scene GameScene { get; set; }
        
        
        [Test()]
        public void TestCase()
        {
            foreach (var caseTuple in Cases)
            {
                var direction = caseTuple.Item2;
                int x         = caseTuple.Item1.X;
                int y         = caseTuple.Item1.Y;

                Assert.IsTrue(
                    GameScene.BrickPicker.GetDetachDirectionForBrick(
                        GameScene.GetActorAtCell<BrickActor>(x, y)
                    ) == direction
                );
            }
        }
    }
}
