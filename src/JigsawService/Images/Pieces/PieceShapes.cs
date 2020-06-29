using System;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using JigsawService.Extensions;

namespace JigsawService.Images.Pieces
{
    internal sealed class PieceShapes : IPieceShapes
    {
        private readonly int unit;

        public int Width => unit * 5;

        private PieceShapes(int unit)
        {
            this.unit = unit;
        }

        public static IPieceShapes Create(int origin)
        {
            return new PieceShapes(origin / 3);
        }

        public IPath Build(Point location, Edges edges)
        {
            return new[]
                   {
                      GenerateEdge(location, edges.Top, 0),
                      GenerateEdge(location, edges.Right, 90),
                      GenerateEdge(location, edges.Bottom, 180),
                      GenerateEdge(location, edges.Left, 270),
                   }
                   .SelectMany(_ => _.LineSegments)
                   .Aggregate(
                       new PathBuilder(),
                       (acc, next) => acc.AddSegment(next))
                   .Build();
        }

        private Path GenerateEdge(Point location, Edge edge,int degree)
        {
            return GenerateEdge(edge)
                   ._(_ => RotateEdge(_, edge, degree))
                   .Translate(location.X - unit, location.Y - unit)
                   as Path;
        }

        private IPath RotateEdge(IPath origin, Edge edge, int degree)
        {
            if (degree == 0) return origin;

            return origin
                   .RotateDegree(degree)
                   .TranslateTo(FindTranslateTarget(degree, edge));

        }

        private PointF FindTranslateTarget(int degree, Edge edge)
        {
            return degree switch
            {
                90 => edge switch
                {
                    Edge.Inside => new PointF(3 * unit, unit),
                    Edge.Outside => new PointF(4 * unit, unit),
                    _ => new PointF(4 * unit, unit),
                },
                180 => edge switch
                {
                    Edge.Inside => new PointF(unit, 3 * unit),
                    Edge.Outside => new PointF(unit, 4 * unit),
                    _ => new PointF(unit, 4 * unit),
                },
                270 => edge switch
                {
                    Edge.Inside => new PointF(unit, unit),
                    Edge.Outside => new PointF(0, unit),
                    _ => new PointF(unit, unit),
                },
                _ => throw new Exception($"Wrong rotate degree value: {degree}"),
            };
        }

        private IPath GenerateEdge(Edge edge)
        {
            var path = new PathBuilder();
            if (edge == Edge.Outside)
            {
                path
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
                    .AddLine(3 * unit, unit, 4 * unit, unit);
            }
            else if (edge == Edge.Inside)
            {
                path
                    .AddLine(unit, unit, 2 * unit, unit)
                    .AddBezier(
                        new PointF(2 * unit, unit),
                        new PointF(3 * unit, 1.5f * unit),
                        new PointF(1.5f * unit, 2 * unit),
                        new PointF(2.5f * unit, 2 * unit))
                    .AddBezier(
                        new PointF(2.5f * unit, 2 * unit),
                        new PointF(3.5f * unit, 2 * unit),
                        new PointF(2 * unit, 1.5f * unit),
                        new PointF(3 * unit, unit))
                    .AddLine(3 * unit, unit, 4 * unit, unit);
            }
            else
            {
                path.AddLine(unit, unit, 4 * unit, unit);
            }
            return path.Build();
        }
    }
}
