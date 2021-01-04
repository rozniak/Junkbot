/**
 * SizeHelper.cs - Size Helper Methods
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
    /// Provides helper methods for the <see cref="Size"/> struct.
    /// </summary>
    internal static class SizeHelper
    {
        /// <summary>
        /// Reduces (divides) the <see cref="Size"/> by a factor.
        /// </summary>
        /// <param name="subject">
        /// The <see cref="Size"/>.
        /// </param>
        /// <param name="factor">
        /// The factor to reduce by.
        /// </param>
        /// <returns>
        /// The result of the operation on the <see cref="Size"/>.
        /// </returns>
        public static Size Reduce(
            this Size subject,
            Size      factor
        )
        {
            return new Size(
                subject.Width / factor.Width,
                subject.Height / factor.Height
            );
        }
    }
}
