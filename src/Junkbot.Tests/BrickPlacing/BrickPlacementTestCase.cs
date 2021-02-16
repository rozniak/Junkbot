/**
 * BrickPlacementTestCase.cs - Junkbot Brick Placing Test Case Data Class
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.World.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Tests.BrickPlacing
{
    /// <summary>
    /// Represents a single piece of test case data for a brick placement test.
    /// </summary>
    public sealed class BrickPlacementTestCase
    {
        /// <summary>
        /// Gets or sets the cell at which to detach a brick.
        /// </summary>
        public Point DetachAt { get; set; }
        
        /// <summary>
        /// Gets or sets the direction to detach the brick.
        /// </summary>
        public BrickDetachDirection Direction { get; set; }
        
        /// <summary>
        /// Gets or sets the expected results of new brick locations versus their
        /// original locations.
        /// </summary>
        public List<Tuple<Point, Point>> ExpectedPlacements { get; set; }
        
        /// <summary>
        /// Gets or sets the cell at which to place the bricks.
        /// </summary>
        public Point PlaceAt { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="BrickPlacementTestCase"/>
        /// class.
        /// </summary>
        public BrickPlacementTestCase()
        {
            DetachAt           = Point.Empty;
            Direction          = BrickDetachDirection.Either;
            ExpectedPlacements = new List<Tuple<Point, Point>>();
            PlaceAt            = Point.Empty;
        }
    }
}
