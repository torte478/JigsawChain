using JigsawService.Templets;
using SixLabors.ImageSharp;

namespace JigsawService.Images
{
    internal interface IImages
    {
        Maybe<Image, string> Load(byte[] image);
        (Image image, Edges[,] edges) BuildPreview(Image image, Templet right);
    }
}