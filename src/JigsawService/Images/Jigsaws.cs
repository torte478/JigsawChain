using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using JigsawService.Templets;
using JigsawService.Extensions;

namespace JigsawService.Images
{
    internal sealed class Jigsaws
    {
        private static readonly Random random = new Random(DateTime.Now.Millisecond);

        private readonly Size size;
        private readonly Size unit;
        private readonly (int x, int y)[] pieces;
        private readonly int total;

        public Jigsaws(Size size, Size unit, (int x, int y)[] pieces, int total)
        {
            this.size = size;
            this.unit = unit;
            this.pieces = pieces;
            this.total = total;
        }

        public static Jigsaws Create(Size size, Size pieces)
        {
            var resized = new Size(
                size.Width - size.Width % pieces.Width,
                size.Height - size.Height % pieces.Height);

            var unit = new Size(
                            resized.Width / pieces.Width,
                            resized.Height / pieces.Height);

            var coordinates = EnumeratePieces(pieces, unit).ToArray();

            var total = unit.Width * unit.Height;

            return new Jigsaws(resized, unit, coordinates, total);
        }

        public Image BuildPreview(Image origin, Templet templet)
        {
            var preview = origin
                            .CloneAs<Rgba32>()
                            .Clone(_ => _.Resize(size));

            FillPieces(preview, templet);
            return preview;
        }

        private void FillPieces(Image<Rgba32> image, Templet templet)
        {
            foreach (var piece in pieces)
            {
                var color = piece
                            ._(_ => GetAverageColor(image, _))
                            .Apply(_ => ApplyRandomTolerance(_, templet.Tolerancy));
                DrawPiece(image, piece, color);
                ApplyNoice(image, templet.Noise, piece);
            }
        }

        private void ApplyNoice(Image<Rgba32> image, int noise, (int x, int y) piece)
        {
            var noised = noise * total / 100.0;
            for (var i = 0; i < noised; ++i)
            {
                var x = piece.x + random.Next(unit.Width);
                var y = piece.y + random.Next(unit.Height);
                image[x, y] = new Rgba32().Apply(_ => (byte)random.Next(256));
            }
        }

        private void DrawPiece(Image<Rgba32> image, (int x, int y) piece, Rgba32 color)
        {
            for (var y = 0; y < unit.Height && piece.y + y < image.Height; ++y)
            {
                var span = image.GetPixelRowSpan(piece.y + y);
                for (var x = 0; x < unit.Width && piece.x + x < image.Width; ++x)
                    span[piece.x + x] = color;
            }
        }

        private Rgba32 GetAverageColor(Image<Rgba32> image, (int x, int y) piece)
        {
            var r = 0;
            var g = 0;
            var b = 0;
            for (var y = 0; y < unit.Height && piece.y + y < image.Height; ++y)
            {
                var span = image.GetPixelRowSpan(piece.y + y);
                for (var x = 0; x < unit.Width && piece.x + x < image.Width; ++x)
                {
                    var pixel = span[piece.x + x];
                    r += pixel.R * pixel.R;
                    g += pixel.G * pixel.G;
                    b += pixel.B * pixel.B;
                }
            }
            var average = new Rgba32(
                (byte)Math.Sqrt(r / total),
                (byte)Math.Sqrt(g / total),
                (byte)Math.Sqrt(b / total));
            return average;
        }

        private static byte ApplyRandomTolerance(byte origin, int tolerance)
        {
            var shift = (int)(tolerance * 255 / 100.0);
            var next = random.Next(2) == 0
                       ? origin + shift
                       : origin - shift;

            if (next < 0)
                next = Math.Abs(next);
            if (next > 255)
                next = 2 * 255 - next;

            return (byte)next;
        }

        private static IEnumerable<(int, int)> EnumeratePieces(Size pieces, Size factor)
        {
            for (var i = 0; i < pieces.Height; ++i)
                for (var j = 0; j < pieces.Width; ++j)
                    yield return (j * factor.Width, i * factor.Height);
        }
    }
}
