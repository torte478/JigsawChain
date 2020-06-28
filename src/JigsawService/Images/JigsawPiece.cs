using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using JigsawService.Extensions;

namespace JigsawService.Images
{
    internal sealed class JigsawPiece : IPiece
    {
        private readonly Image<Rgba32> origin;
        private readonly Point location;
        private readonly IPath shape;
        private readonly Lazy<int> count;

        public int Count => count.Value;

        public Size Canvas { get; set; }

        public JigsawPiece(
                    Image<Rgba32> origin, 
                    Point location, 
                    IPath shape, 
                    Size canvas)
        {
            this.origin = origin;
            this.location = location;
            this.shape = shape;
            count = new Lazy<int>(() => ToPixels().Length);
            Canvas = canvas;
        }

        public IPiece Draw(Image<Rgba32> image)
        {
            using var resized = image.Clone(_ => _.Resize(Canvas));
            var brush = new ImageBrush(resized);
            origin.Mutate(_ => _.Fill(brush, shape));
            return this;
        }

        public Rgba32[] ToPixels()
        {
            var pixels = new List<Rgba32>();
            using var mask = new Image<Rgba32>(Canvas.Width, Canvas.Height);
            mask.Mutate(_ => _.Fill(
                                Color.Red,
                                shape.TranslateTo(new PointF(0, 0))));

            var masked = Color.Red.ToPixel<Rgba32>();
            for (var i = 0; i < mask.Height; ++i)
            for (var j = 0; j < mask.Width; ++j)
            {
                if (mask[j, i] == masked)
                    pixels.Add(origin[location.X + j, location.Y + i]);
            }   
            return pixels.ToArray();
        }
    }
}
