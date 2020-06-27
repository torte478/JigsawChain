﻿using System;
using System.IO;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using JigsawService.Extensions;
using JigsawService.Templets;

namespace JigsawService
{
    internal sealed class Images : IImages
    {
        private readonly IImageDecoder decoder;
        private readonly ((int min, int max) width, (int min, int max) height) limitations;
        private readonly ILogger<Images> logger;

        public Images(IImageDecoder decoder, ((int, int), (int, int)) limitations, ILogger<Images> logger)
        {
            this.decoder = decoder;
            this.limitations = limitations;
            this.logger = logger;
        }

        public Image BuildPreview(Image origin, Templet templet)
        {
            return origin; //TODO
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
            return image.Width >= limitations.width.min
                   && image.Width <= limitations.width.max
                   && image.Height >= limitations.height.min
                   && image.Height <= limitations.height.max;
        }
    }
}