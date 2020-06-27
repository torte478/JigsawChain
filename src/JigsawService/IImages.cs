using JigsawService.Templets;
using SixLabors.ImageSharp;

namespace JigsawService
{
    internal interface IImages
    {
        Maybe<Image, string> Load(byte[] image);
        Image BuildPreview(Image image, Templet right);
    }
}