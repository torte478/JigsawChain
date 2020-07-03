using System;
using System.IO;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using JigsawService.Services;

namespace JigsawService.Fake
{
    internal sealed class Input : IInput
    {
        private readonly ILogger<Input> logger;

        public event Action<IRpcToken, byte[]> UploadJigsaw;
        public event Action<IRpcToken, string, string> ChooseTemplet;
        public event Action<IRpcToken, string> ConfirmJigsaw;

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

        public void RaiseConfirmJigsawEvent(string id)
        {
            ConfirmJigsaw.Invoke(null, id);
        }

        public IInput SendError(IRpcToken token, string message)
        {
            logger.LogError($"Error: {message}");
            return this;
        }

        public IInput SendTemplet(IRpcToken token, string id, string templet)
        {
            logger.LogDebug($"Response: {id} {templet}");
            return this;
        }

        public IInput SendPreview(IRpcToken token, string id, Image preview, int cost)
        {
            preview.Save($"d:\\jigsawChain\\images\\output\\1.jpg");
            logger.LogDebug($"Responce: {id} {cost}");
            return this;
        }

        public IInput SendConfirmation(IRpcToken token, string id)
        {
            logger.LogDebug($"Response: {id}");
            return this;
        }
    }
}
