using System.Drawing;

namespace Junkbot.Helpers
{
    internal static class PointHelper
    {
        public static Point Add(this Point origin, Point delta)
        {
            return new Point(
                origin.X + delta.X,
                origin.Y + delta.Y
                );
        }

        public static Point Reduce(this Point origin, Size factor)
        {
            return new Point(
                origin.X / factor.Width,
                origin.Y / factor.Height
                );
        }

        public static Point Product(this Point origin, Size factor)
        {
            return new Point(
                origin.X * factor.Width,
                origin.Y * factor.Height
                );
        }

        public static Point Subtract(this Point origin, int delta)
        {
            return new Point(
                origin.X - delta,
                origin.Y - delta
                );
        }

        public static Point Subtract(this Point origin, Point delta)
        {
            return new Point(
                origin.X - delta.X,
                origin.Y - delta.Y
                );
        }

        public static Point PointToGrid(this Point origin)
        {
            return origin.Reduce(new Size(15, 18));
        }
    }
}
