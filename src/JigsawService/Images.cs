using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using JigsawService.Extensions;
using JigsawService.Templets;

namespace JigsawService
{
    internal sealed class Images : IImages
    {
        private readonly IImageDecoder decoder;
        private readonly (Size min, Size max) limitations;
        private readonly (Size size, Size pieces) prototype;
        private readonly ILogger<Images> logger;

        public Images(
                IImageDecoder decoder, 
                (Size, Size) limitations, 
                (Size, Size) prototype, 
                ILogger<Images> logger)
        {
            this.decoder = decoder;
            this.limitations = limitations;
            this.prototype = prototype;
            this.logger = logger;
        }

        public Image BuildPreview(Image origin, Templet templet)
        {
            var size = new Size(
                prototype.size.Width - prototype.size.Width % prototype.pieces.Width,
                prototype.size.Height - prototype.size.Height % prototype.pieces.Height);
            var preview = origin
                            .CloneAs<Rgba32>()
                            .Clone(_ => _.Resize(size));

            var factor = new Size(
                            preview.Width / prototype.pieces.Width, 
                            preview.Height / prototype.pieces.Height);
            var pieces = Enumerable
                            .Range(0, prototype.pieces.Height)
                            .SelectMany(i => Enumerable
                                             .Range(0, prototype.pieces.Width)
                                             .Select(j => (
                                             j * factor.Width,
                                             i * factor.Height)));
            FillPieces(preview, pieces, factor);
            return preview;
        }

        private void FillPieces(
                        Image<Rgba32> image, 
                        IEnumerable<(int x, int y)> pieces, 
                        Size size)
        {
            foreach (var piece in pieces)
            {
                var r = 0;
                var g = 0;
                var b = 0;
                var total = 0;
                for (var x = 0; x < size.Width && piece.x + x < image.Width; ++x)
                for (var y = 0; y < size.Height && piece.y + y < image.Height; ++y)
                {
                    var pixel = image[piece.x + x, piece.y + y];
                    r += pixel.R * pixel.R;
                    g += pixel.G * pixel.G;
                    b += pixel.B * pixel.B;
                    ++total;
                }
                var color = new Rgba32(
                    (byte)Math.Sqrt(r / total),
                    (byte)Math.Sqrt(g / total),
                    (byte)Math.Sqrt(b / total));

                for (var y = 0; y < size.Height && piece.y + y < image.Height; ++y)
                {
                    var span = image.GetPixelRowSpan(piece.y + y);
                    for (var x = 0; x < size.Width && piece.x + x < image.Width; ++x)
                        span[piece.x + x] = color;
                }
            }
        }

        public Maybe<Image, string> Load(byte[] image)
        {
            using var stream = new MemoryStream(image);
            try
            {
                var decoded = decoder.Decode(Configuration.Default, stream);

                if (!RequiresLimitations(decoded))
                    return A<Image, string>.Left("Image has wrong size");

                return decoded._(A<Image, string>.Right);
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Image decoding error: {ex.Message}");
                return A<Image, string>.Left("Wrong image format");
            }
        }

        private bool RequiresLimitations(IImage image)
        {
            return image.Width >= limitations.min.Width
                   && image.Width <= limitations.max.Width
                   && image.Height >= limitations.min.Height
                   && image.Height <= limitations.max.Height;
        }
    }
}