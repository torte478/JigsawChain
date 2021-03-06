﻿using System;
using System.IO;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using JigsawService.Extensions;

namespace JigsawService.Images
{
    internal sealed class RawImages
    {
        private readonly IImageDecoder decoder;
        private readonly (Size min, Size max) limitations;
        private readonly ILogger<RawImages> logger;

        public RawImages(
                    IImageDecoder decoder, 
                    (Size min, Size max) limitations,
                    ILogger<RawImages> logger)
        {
            this.decoder = decoder;
            this.limitations = limitations;
            this.logger = logger;
        }

        public Maybe<Image, string> Load(byte[] image)
        {
            using var stream = new MemoryStream(image);
            try
            {
                var decoded = decoder.Decode(Configuration.Default, stream);

                if (!RequiresLimitations(decoded))
                    return "Image has wrong size".Left<Image, string>();

                return decoded.Right<Image, string>();
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Image decoding error: {ex.Message}");
                return "Wrong image format".Left<Image, string>();
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
