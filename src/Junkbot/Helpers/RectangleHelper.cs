/**
 * RectangleHelper.cs - Rectangle Helper Methods
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using System.Drawing;

namespace Junkbot.Helpers
{
    /// <summary>
    /// Provides helper methods for the <see cref="Rectangle"/> struct.
    /// </summary>
    internal static class RectangleHelper
    {
        /// <summary>
        /// Adds a position offset to the <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rect">
        /// The <see cref="Rectangle"/>.
        /// </param>
        /// <param name="delta">
        /// The <see cref="Point"/> to add.
        /// </param>
        /// <returns>
        /// The result of the operation on the <see cref="Rectangle"/>.
        /// </returns>
        public static Rectangle Add(
            this Rectangle rect,
            Point          delta
        )
        {
            return new Rectangle(
                rect.Location.Add(delta),
                rect.Size
            );
        }
        
        /// <summary>
        /// Subtracts a position offset from the <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rect">
        /// The <see cref="Rectangle"/>.
        /// </param>
        /// <param name="delta">
        /// The <see cref="Point"/> to subtract.
        /// </param>
        /// <returns>
        /// The result of the operation on the <see cref="Rectangle"/>.
        /// </returns>
        public static Rectangle Subtract(
            this Rectangle rect,
            Point          delta
        )
        {
            return new Rectangle(
                rect.Location.Subtract(delta),
                rect.Size
            );
        }
    }
}
