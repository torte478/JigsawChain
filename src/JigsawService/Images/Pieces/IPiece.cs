using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace JigsawService.Images.Pieces
{
    internal interface IPiece
    {
        int Count { get; }
        Size Canvas { get; }
        Edges Edges { get; }
        Rgba32[] ToPixels();
        IPiece Draw(Image<Rgba32> image);
    }
}
