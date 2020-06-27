using System;
using System.IO;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using JigsawService.Extensions;
using JigsawService.Templets;
using SixLabors.ImageSharp.Processing;
using System.Linq;

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
            var preview = origin.Clone(_ => _.Resize(prototype.size.Width, prototype.size.Height));

            var size = new Size(
                            preview.Width / prototype.pieces.Width, 
                            preview.Height / prototype.pieces.Height);
            var pieces = Enumerable
                            .Range(0, prototype.pieces.Height)
                            .SelectMany(i => Enumerable
                                             .Range(0, prototype.pieces.Width)
                                             .Select(j => (
                                             j * size.Width,
                                             i * size.Height)));

            return preview;
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