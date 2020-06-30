using System.Threading.Tasks;
using JigsawService.Extensions;
using SixLabors.ImageSharp;

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
                                  ? A<bool, string>.Right(true)
                                  : A<bool, string>.Left("Can't store image"));
        }
    }
}
