using SixLabors.ImageSharp;
using System.Threading.Tasks;

namespace JigsawService
{
    internal interface IImageService
    {
        Task<Maybe<bool, string>> SaveImageAsync(Image image);
    }
}