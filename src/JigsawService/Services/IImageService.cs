using SixLabors.ImageSharp;
using System.Threading.Tasks;

namespace JigsawService.Services
{
    internal interface IImageService
    {
        Task<Maybe<bool, string>> SaveImageAsync(Image image);
    }
}