using System;
using System.IO;
using SixLabors.ImageSharp;

namespace JigsawService.Fake
{
    internal sealed class User : IUser
    {
        public event Action<IRpcToken, byte[]> UploadJigsaw;

        public void RaiseUploadJigsawEvent(string path)
        {
            using (var image = Image.Load(path))
            {
                using (var stream = new MemoryStream())
                {
                    var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder();
                    image.Save(stream, encoder);
                    var bytes = stream.ToArray();

                    UploadJigsaw.Invoke(null, bytes);
                }
            }
        }
    }
}
