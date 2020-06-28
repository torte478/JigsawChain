using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;

namespace JigsawService.Extensions
{
    internal static class IPathExtensions
    {
        public static IPath TranslateTo(this IPath origin, PointF position)
        {
            return origin.Translate(
                    position.X - origin.Bounds.X,
                    position.Y - origin.Bounds.Y);
        }
    }
}
