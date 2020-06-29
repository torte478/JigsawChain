using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;

namespace JigsawService.Images
{
    internal sealed class RandomEdges
    {
        private readonly Random random = new Random((int)DateTime.Now.Ticks);

        public IEnumerable<(int, int, Edges)> Generate(Size count)
        {
            var edges = new Edges[count.Height, count.Width];
            for (var i = 0; i < count.Height; ++i)
            for (var j = 0; j < count.Width; ++j)
            {
                edges[i, j] = new Edges(
                    top: i == 0 ? Edge.Flat : GetOppositeTo(edges[i - 1, j].Bottom),
                    right: j == count.Width - 1 ? Edge.Flat : GetRandomEdge(),
                    bottom: i == count.Height - 1 ? Edge.Flat : GetRandomEdge(),
                    left: j == 0 ? Edge.Flat : GetOppositeTo(edges[i, j - 1].Right));
                yield return (i, j, edges[i, j]);
            }
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
