/**
 * PointHelper.cs - Point Helper Methods
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
    /// Provides helper methods for the <see cref="Point"/> struct.
    /// </summary>
    internal static class PointHelper
    {
        /// <summary>
        /// Adds one <see cref="Point"/> to another.
        /// </summary>
        /// <param name="origin">
        /// The first <see cref="Point"/>.
        /// </param>
        /// <param name="delta">
        /// The second <see cref="Point"/> (difference).
        /// </param>
        /// <returns>
        /// The result of the operation on the <see cref="Point"/>.
        /// </returns>
        public static Point Add(
            this Point origin,
            Point      delta
        )
        {
            return new Point(
                origin.X + delta.X,
                origin.Y + delta.Y
            );
        }
        
        /// <summary>
        /// Reduces (divides) the <see cref="Point"/> by a factor.
        /// </summary>
        /// <param name="origin">
        /// The <see cref="Point"/>.
        /// </param>
        /// <param name="factor">
        /// The factor to reduce by.
        /// </param>
        /// <returns>
        /// The result of the operation on the <see cref="Point"/>.
        /// </returns>
        public static Point Reduce(
            this Point origin,
            Size       factor
        )
        {
            return new Point(
                origin.X / factor.Width,
                origin.Y / factor.Height
            );
        }
        
        /// <summary>
        /// Multiplies the <see cref="Point"/> by a factor.
        /// </summary>
        /// <param name="origin">
        /// The <see cref="Point"/>.
        /// </param>
        /// <param name="factor">
        /// The factor to multiply by.
        /// </param>
        /// <returns>
        /// The result of the operation on the <see cref="Point"/>.
        /// </returns>
        public static Point Product(
            this Point origin,
            Size       factor
        )
        {
            return new Point(
                origin.X * factor.Width,
                origin.Y * factor.Height
            );
        }
        
        /// <summary>
        /// Subtracts the specified offset from a <see cref="Point"/>.
        /// </summary>
        /// <param name="origin">
        /// The <see cref="Point"/>.
        /// </param>
        /// <param name="delta">
        /// The offset to subtract.
        /// </param>
        /// <returns>
        /// The result of the operation on the <see cref="Point"/>.
        /// </returns>
        public static Point Subtract(
            this Point origin,
            int        delta
        )
        {
            return new Point(
                origin.X - delta,
                origin.Y - delta
            );
        }
        
        /// <summary>
        /// Subtract one <see cref="Point"/> from another.
        /// </summary>
        /// <param name="origin">
        /// The <see cref="Point"/>.
        /// </param>
        /// <param name="delta">
        /// The offset <see cref="Point"/> subtract.
        /// </param>
        /// <returns>
        /// The result of the operation on the <see cref="Point"/>.
        /// </returns>
        public static Point Subtract(
            this Point origin,
            Point      delta
        )
        {
            return new Point(
                origin.X - delta.X,
                origin.Y - delta.Y
            );
        }
    }
}
