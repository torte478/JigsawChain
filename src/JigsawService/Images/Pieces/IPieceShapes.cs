using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;

namespace JigsawService.Images.Pieces
{
    internal interface IPieceShapes
    {
        int Width { get; }
        IPath Build(Point location, Edges edges);
    }
}
