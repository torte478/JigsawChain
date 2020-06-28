using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System;
using System.Linq;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            using var image = Image.Load(@"d:\jigsawChain\images\input\1.jpg");
            image.Mutate(_ => _.Resize(500, 500));

            var path = BuildPath(new Size(500, 500));
            var x = path.Contains(new PointF(0, 0));
            var y = path.Contains(new PointF(200, 200));
            using var result = image.Clone(_ => { });
            var brush = new ImageBrush(image);
            result.Mutate(_ => _.Fill(Color.Green).Fill(brush, path));

            result.Save(@"d:\jigsawChain\images\output\2.jpg");
        }

        private static IPath BuildPath(Size size)
        {
            var unit = size.Width / 5;

            //var top = new PathBuilder()
            //          .AddLine(unit, unit, 2 * unit, unit)
            //          .AddBezier(
            //            new PointF(2 * unit, unit),
            //            new PointF(3 * unit, 1.5f * unit),
            //            new PointF(1.5f * unit, 2 * unit),
            //            new PointF(2.5f * unit, 2 * unit))
            //          .AddBezier(
            //            new PointF(2.5f * unit, 2 * unit),
            //            new PointF(3.5f * unit, 2 * unit),
            //            new PointF(2 * unit, 1.5f * unit),
            //            new PointF(3 * unit, unit))
            //          .AddLine(3 * unit, unit, 4 * unit, unit)
            //          .Build();

            //var right = top.RotateDegree(90);
            //right = right.Translate(3 * unit - right.Bounds.X, unit - right.Bounds.Y);

            //var bottom = top.RotateDegree(180);
            //bottom = bottom.Translate(unit - bottom.Bounds.X, 3 * unit - bottom.Bounds.Y);

            //var left = top.RotateDegree(270);
            //left = left.Translate(unit - left.Bounds.X, unit - left.Bounds.Y);

            var top = new PathBuilder()
                      .AddLine(unit, unit, 2 * unit, unit)
                      .AddBezier(
                        new PointF(2 * unit, unit),
                        new PointF(3 * unit, 0.5f * unit),
                        new PointF(1.5f * unit, 0),
                        new PointF(2.5f * unit, 0))
                      .AddBezier(
                        new PointF(2.5f * unit, 0),
                        new PointF(3.5f * unit, 0),
                        new PointF(2 * unit, 0.5f * unit),
                        new PointF(3 * unit, unit))
                      .AddLine(3 * unit, unit, 4 * unit, unit)
                      .Build();

            var right = top.RotateDegree(90);
            right = right.Translate(4 * unit - right.Bounds.X, unit - right.Bounds.Y);

            var bottom = top.RotateDegree(180);
            bottom = bottom.Translate(unit - bottom.Bounds.X, 4 * unit - bottom.Bounds.Y);

            var left = top.RotateDegree(270);
            left = left.Translate(0 - left.Bounds.X, unit - left.Bounds.Y);

            var path = new PathBuilder();
            var segments = new[] { top, right, bottom, left }
                           .Select(p => p as Path)
                           .SelectMany(p => p.LineSegments);
            foreach (var segment in segments)
                path.AddSegment(segment);

            return path.Build();
        }
    }
}
