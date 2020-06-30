using System.Threading.Tasks;
using SixLabors.ImageSharp;
using JigsawService.Extensions;

namespace JigsawService.Fake
{
    internal sealed class ImageService : IImageService
    {
        private readonly bool right;

        public ImageService(bool right)
        {
            this.right = right;
        }

        public Task<Maybe<bool, string>> SaveImageAsync(Image image)
        {
            return Task.Run(() => right
                                  ? true.Right<bool, string>()
                                  : "Can't store image".Left<bool, string>());
        }
    }
}
