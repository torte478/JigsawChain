using System;
using SixLabors.ImageSharp;
using JigsawService.Templets;

namespace JigsawService.Images
{
    internal sealed class Images : IImages
    {
        private readonly Func<byte[], Maybe<Image, string>> loadImage;
        private readonly Func<Image, Templet, Image> buildPreview;

        public Images(
                    Func<byte[], Maybe<Image, string>> loadImage,
                    Func<Image, Templet, Image> buildPreview) 
        {
            this.loadImage = loadImage;
            this.buildPreview = buildPreview;
        }

        public Maybe<Image, string> Load(byte[] image)
        {
            return loadImage(image);
        }

        public Image BuildPreview(Image origin, Templet templet)
        {
            return buildPreview(origin, templet);
        }

        
    }
}