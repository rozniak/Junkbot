using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Helpers
{
    internal static class RectangleHelper
    {
        public static Point[] ExpandToGridCoordinates(this Rectangle rect)
        {
            var coords = new List<Point>();

            for (int h = 0; h < rect.Height; h++)
            {
                for (int w = 0; w < rect.Width; w++)
                {
                    coords.Add(new Point(rect.X + w, rect.Y + h));
                }
            }

            return coords.ToArray();
        }

        public static Rectangle PointToGrid(this Rectangle rect)
        {
            return new Rectangle(
                rect.Location.PointToGrid(),
                rect.Size.PointToGrid()
                );
        }
    }
}
