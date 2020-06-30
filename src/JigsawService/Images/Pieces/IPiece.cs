using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace JigsawService.Images.Pieces
{
    internal interface IPiece
    {
        Point JigsawPosition { get; }
        int Count { get; }
        Size Canvas { get; }
        Edges Edges { get; }
        Rgba32[] ToPixels();
        IPiece Draw(Image<Rgba32> image);
        IPiece Draw(Rgba32 color);
    }
}
