using SixLabors.ImageSharp;

namespace JigsawService
{
    internal interface IImages
    {
        Maybe<IImage, string> Load(byte[] image);
    }
}