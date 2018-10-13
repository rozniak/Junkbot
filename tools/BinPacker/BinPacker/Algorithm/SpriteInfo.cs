using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Tools.BinPacker.Algorithm
{
    /// <summary>
    /// Represents information about a sprite within an atlas.
    /// </summary>
    internal sealed class SpriteInfo
    {
        /// <summary>
        /// Gets the name of the sprite.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the bounding box coordinates of the sprite within the atlas in which
        /// it resides.
        /// </summary>
        public Rectangle Bounds { get; private set; }


        /// <summary>
        /// Initializes a new instance of the SpriteInfo class with a name and bounding
        /// box.
        /// </summary>
        /// <param name="name">The name of the sprite.</param>
        /// <param name="bounds">The bounding box coordinates of the sprite.</param>
        public SpriteInfo(string name, Rectangle bounds)
        {
            Name = name;
            Bounds = bounds;
        }
    }
}
