using SixLabors.ImageSharp;
using System.IO;

namespace JigsawService.Fake
{
    internal sealed class Simulation : ISimulation
    {
        private readonly User user;

        public Simulation(User user)
        {
            this.user = user;
        }

        public void UserUploadJigsaw(string path)
        {
            using (var image = Image.Load(path))
            {
                using (var stream = new MemoryStream())
                {
                    var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder();
                    image.Save(stream, encoder);
                    var bytes = stream.ToArray();

                    user.RaiseUploadJigsawEvent(null, bytes);
                }
            }
        }
    }
}