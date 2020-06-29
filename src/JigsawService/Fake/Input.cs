using System;
using System.IO;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace JigsawService.Fake
{
    internal sealed class Input : IInput
    {
        private readonly ILogger<Input> logger;

        public event Action<IRpcToken, byte[]> UploadJigsaw;
        public event Action<IRpcToken, string, string> ChooseTemplet;

        public Input(ILogger<Input> logger)
        {
            this.logger = logger;
        }

        public void RaiseUploadJigsawEvent(string path)
        {
            var bytes = File.ReadAllBytes(path);
            UploadJigsaw.Invoke(null, bytes);
        }

        public void RaiseChooseTempletEvent(string id, int tolerancy, int noise)
        {
            var templet = "{\"tolerancy\":"
                          + tolerancy.ToString()
                          + ",\"noise\":"
                          + noise.ToString()
                          + "}";
            ChooseTemplet.Invoke(null, id, templet);
        }

        public IInput SendError(IRpcToken token, string message)
        {
            logger.LogInformation($"Error: {message}");
            return this;
        }

        public void SendTemplet(IRpcToken token, string id, string templet)
        {
            logger.LogDebug($"Response: {id} {templet}");
        }

        public void SendPreview(IRpcToken token, string id, Image preview, int cost)
        {
            preview.Save($"d:\\jigsawChain\\images\\output\\1.jpg");
            logger.LogDebug($"Responce: {id} {cost}");
        }
    }
}
