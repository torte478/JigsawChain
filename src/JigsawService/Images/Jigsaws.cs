using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using JigsawService.Extensions;
using JigsawService.Images.Pieces;
using JigsawService.Templets;

namespace JigsawService.Images
{
    internal sealed class Jigsaws
    {
        private static readonly Random random = new Random(DateTime.Now.Millisecond);

        private readonly Size pieces;
        private readonly Func<Image<Rgba32>, Size, IEnumerable<IPiece>> buildPieces;

        public Size Size { get; }

        private Jigsaws(
                    Size size, 
                    Size pieces,
                    Func<Image<Rgba32>, Size, IEnumerable<IPiece>> buildPieces)
        {
            Size = size;
            this.pieces = pieces;
            this.buildPieces = buildPieces;
        }

        public static Jigsaws Create(
                        Size size,
                        Size pieces,
                        Func<Image<Rgba32>, Size, IEnumerable<IPiece>> buildPieces)
        {
            var side = Math.Min(
                            size.Width / pieces.Width, 
                            size.Height / pieces.Height);
            var resized = new Size(side * pieces.Width,side * pieces.Height);

            return new Jigsaws(resized, pieces, buildPieces);
        }

        public Image BuildPreview(Image origin, Templet templet)
        {
            var preview = origin
                            .CloneAs<Rgba32>()
                            .Clone(_ => _.Resize(Size));

            foreach (var piece in buildPieces(preview, pieces))
                Fill(piece, templet);

            ApplyNoice(preview, templet.Noise);
            return preview;
        }

        private void Fill(IPiece piece, Templet templet)
        {
            var color = piece
                        ._(GetAverageColor)
                        .ForEachComponent(
                            _ => ApplyRandomTolerance(_, templet.Tolerancy))
                        ._(piece.Draw);
        }

        private Rgba32 GetAverageColor(IPiece piece)
        {
            var r = 0;
            var g = 0;
            var b = 0;
            foreach (var pixel in piece.ToPixels())
            {
                r += pixel.R * pixel.R;
                g += pixel.G * pixel.G;
                b += pixel.B * pixel.B;
            }
            return new Rgba32(
                (byte)Math.Sqrt(r / piece.Count),
                (byte)Math.Sqrt(g / piece.Count),
                (byte)Math.Sqrt(b / piece.Count));
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

        private Image<Rgba32> ApplyNoice(Image<Rgba32> image, int noise)
        {
            var noised = noise * (image.Width * image.Height) / 100.0;
            for (var i = 0; i < noised; ++i)
            {
                var x = random.Next(image.Width);
                var y = random.Next(image.Height);
                image[x, y] = new Rgba32()
                              .ForEachComponent(_ => (byte)random.Next(256));
            }
            return image;
        }
    }
}
