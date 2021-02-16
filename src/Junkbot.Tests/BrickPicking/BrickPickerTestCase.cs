/**
 * BrickPickerTestCase.cs - Junkbot Brick Picking Test Case Data Class
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.World.Logic;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Tests.BrickPicking
{
    /// <summary>
    /// Represents a single piece of test case data for a brick picker test.
    /// </summary>
    public sealed class BrickPickerTestCase
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
        /// Gets or sets the bricks that were expected to be picked up.
        /// </summary>
        public List<Point> ExpectedBricks { get; set; }
        
        /// <summary>
        /// Gets or sets the value that indicates whether the detach attempt should be
        /// blocked.
        /// </summary>
        public bool ShouldBeBlocked { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="BrickPickerTestCase"/>
        /// class.
        /// </summary>
        public BrickPickerTestCase()
        {
            DetachAt        = Point.Empty;
            Direction       = BrickDetachDirection.Either;
            ExpectedBricks  = new List<Point>();
            ShouldBeBlocked = false;
        }
    }
}
