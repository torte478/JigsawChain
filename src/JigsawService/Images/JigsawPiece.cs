using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using JigsawService.Extensions;

namespace JigsawService.Images
{
    internal sealed class JigsawPiece : IPiece
    {
        private readonly Point location;
        private readonly Image<Rgba32> origin;
        private readonly IPath shape;
        private readonly Lazy<int> count;

        public int Count => count.Value;

        public Size Canvas { get; }

        private JigsawPiece(
                    Point location, 
                    Size canvas, 
                    Image<Rgba32> origin, 
                    IPath shape, 
                    Lazy<int> count)
        {
            this.location = location;
            this.origin = origin;
            this.shape = shape;
            this.count = count;
            Canvas = canvas;
        }

        public static IPiece Create(
                                Image<Rgba32> origin, 
                                Point location, 
                                Size size, 
                                IPath shape)
        {
            throw new System.NotImplementedException();
        }

        public IPiece Draw(Image<Rgba32> image)
        {
            throw new System.NotImplementedException();
        }

        public Rgba32[] ToPixels()
        {
            throw new System.NotImplementedException();
        }
    }
}
