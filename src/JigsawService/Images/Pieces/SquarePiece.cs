using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using JigsawService.Extensions;


namespace JigsawService.Images.Pieces
{
    internal sealed class SquarePiece : IPiece
    {
        private readonly Image<Rgba32> origin;
        private readonly Point start;

        public int Count => Canvas.Width * Canvas.Height;

        public Size Canvas { get; }

        public Edges Edges { get;  }

        public SquarePiece(Image<Rgba32> origin, Point start, Size size)
        {
            this.origin = origin;
            this.start = start;
            Canvas = size;
            Edges = new Edges(Edge.Flat, Edge.Flat, Edge.Flat, Edge.Flat);
        }

        public static IEnumerable<IPiece> DecomposeImage(
                                                Image<Rgba32> image, 
                                                Size pieces)
        {
            var size = new Size(
                            image.Width / pieces.Width, 
                            image.Height / pieces.Height);

            for (var i = 0; i < pieces.Height; ++i)
                for (var j = 0; j < pieces.Width; ++j)
                    yield return new Point(j * size.Width, i * size.Height)
                                 ._(_ => new SquarePiece(image, _, size));
        }

        public Rgba32[] ToPixels()
        {
            var pixels = new Rgba32[Count];
            var index = 0;
            for (var i = 0; i < Canvas.Height; ++i)
            {
                var span = origin.GetPixelRowSpan(start.Y + i);
                for (var j = 0; j < Canvas.Width; ++j)
                    pixels[index++] = span[start.X + j];
            }
            return pixels;
        }

        public IPiece Draw(Image<Rgba32> image)
        {
            var canvas = image.Clone(_ => _.Resize(Canvas));
            for (var i = 0; i < Canvas.Height; ++i)
            {
                var from = image.GetPixelRowSpan(i);
                var to = origin.GetPixelRowSpan(start.Y + i);
                for (var j = 0; j < Canvas.Width; ++j)
                    to[start.X + j] = from[j];
            }
            return this;
        }

        public IPiece Draw(Rgba32 color)
        {
            origin.Mutate(_ => _.Fill(color, new RectangleF(start, Canvas)));
            return this;
        }
    }
}
