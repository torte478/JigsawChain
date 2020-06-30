using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace JigsawService.Images.Pieces
{
    internal sealed class JigsawPieces
    {
        private readonly Func<
                            Size, 
                            IEnumerable<(int, int, Edges)>> generateEdges;
        private readonly Func<int, IPieceShapes> buildShapes;

        public JigsawPieces(
                  Func<Size, IEnumerable<(int, int, Edges)>> generateEdges, 
                  Func<int, IPieceShapes> buildShapes)
        {
            this.generateEdges = generateEdges;
            this.buildShapes = buildShapes;
        }

        public IEnumerable<IPiece> Build(Image<Rgba32> image, Size pieces)
        {
            var size = new Size(
                            image.Width / pieces.Width, 
                            image.Height / pieces.Height);

            var shapes = buildShapes(size.Width);
            foreach (var (i, j, edges) in generateEdges(new Size(pieces)))
            {
                var location = new Point(j * size.Width, i * size.Height);
                var piece =  new JigsawPiece(
                    origin: image,
                    location: location,
                    jigsawPosition: new Point(j, i),
                    shape: shapes.Build(location, edges),
                    canvas: new Size(shapes.Width, shapes.Width),
                    edges: edges);

                yield return piece;
            }
        }
    }
}
