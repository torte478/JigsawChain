using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace JigsawService.Images
{
    internal interface IPiece
    {
        int Count { get; }
        Size Canvas { get; }
        Rgba32[] ToPixels();
        IPiece Draw(Image<Rgba32> image);
    }
}
