using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using JigsawService.Extensions;

namespace JigsawService.Images
{
    internal sealed class JigsawPieces
    {
        private readonly Random random = new Random((int)DateTime.Now.Ticks);

        public IEnumerable<IPiece> Build(Image<Rgba32> image, Size pieces)
        {
            var size = new Size(
                            image.Width / pieces.Width, 
                            image.Height / pieces.Height);

            var edges = GenerateEdges(pieces.Width, pieces.Height);
            for (var i = 0; i < pieces.Height; ++i)
            for (var j = 0; j < pieces.Width; ++j)
            {
                var location = new Point(j * size.Width, i * size.Height);
                yield return JigsawPiece.Create(
                    origin: image,
                    location: location,
                    size: size,
                    shape: GenerateShape(location, edges[i, j], size.Width));
            }
        }

        private static IPath GenerateShape(Point location, Edges edges, int width)
        {
            return new[]
                   {
                      GenerateEdge(location, edges.Top, 0, width),
                      GenerateEdge(location, edges.Right, 90, width),
                      GenerateEdge(location, edges.Bottom, 180, width),
                      GenerateEdge(location, edges.Left, 270, width),
                   }
                   .SelectMany(_ => _.LineSegments)
                   .Aggregate(
                       new PathBuilder(),
                       (acc, next) => acc.AddSegment(next))
                   .Build();
        }

        private static Path GenerateEdge(
                                Point location, 
                                Edge edge, 
                                int degree, 
                                int width)
        {
            var unit = width / 5;
            return GenerateEdge(unit, edge)
                   ._(_ => RotateEdge(_, unit, edge, degree))
                   .Translate(location.X - unit, location.Y - unit)
                   as Path;
        }

        private static IPath RotateEdge(IPath origin, int unit, Edge edge, int degree)
        {
            if (degree == 0) return origin;

            var target = FindTranslateTarget(unit, degree, edge);
            return origin
                   .RotateDegree(degree)
                   ._(_ => _.Translate(target.X - _.Bounds.X, target.Y - _.Bounds.Y));

        }

        private static PointF FindTranslateTarget(int unit, int degree, Edge edge)
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
                    _ => new PointF(unit, 4  * unit),
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

        private static IPath GenerateEdge(int unit, Edge edge)
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

        private Edges[,] GenerateEdges(int width, int height)
        {
            var edges = new Edges[height, width];

            for (var i = 0; i < height; ++i)
            for (var j = 0; j < width; ++j)
            {
                edges[i, j] = new Edges(
                    top: i == 0 ? Edge.Flat : GetOppositeTo(edges[i - 1, j].Bottom),
                    right: j == width - 1 ? Edge.Flat : GetRandomEdge(),
                    bottom: i == height - 1 ? Edge.Flat : GetRandomEdge(),
                    left: j == 0 ? Edge.Flat : GetOppositeTo(edges[i, j - 1].Right));
            }

            return edges;
        }

        private Edge GetRandomEdge()
        {
            return (Edge)random.Next(2);
        }

        private static Edge GetOppositeTo(Edge edge)
        {
            if (edge == Edge.Flat) return Edge.Flat;

            return edge == Edge.Inside
                   ? Edge.Outside
                   : Edge.Inside;
        }
    }
}
