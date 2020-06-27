using System;
using System.IO;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using JigsawService.MaybeExtensions;

namespace JigsawService
{
    internal sealed class Images : IImages
    {
        private readonly IImageDecoder decoder;
        private readonly ILogger<Images> logger;

        public Images(IImageDecoder decoder, ILogger<Images> logger)
        {
            this.decoder = decoder;
            this.logger = logger;
        }

        public Maybe<IImage, string> Load(byte[] image)
        {
            using var stream = new MemoryStream(image);
            try
            {
                return decoder
                        .Decode(Configuration.Default, stream)
                        ._(A<IImage, string>.Right);
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Image decoding error: {ex.Message}");
                return A<IImage, string>.Left("Wrong image format");
            }
        }
    }
}