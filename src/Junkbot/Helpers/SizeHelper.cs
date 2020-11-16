using System.Drawing;

namespace Junkbot.Helpers
{
    internal static class SizeHelper
    {
        public static Size Reduce(this Size subject, Size factor)
        {
            return new Size(
                subject.Width / factor.Width,
                subject.Height / factor.Height
                );
        }

        public static Size PointToGrid(this Size subject)
        {
            return subject.Reduce(new Size(15, 18));
        }
    }
}
