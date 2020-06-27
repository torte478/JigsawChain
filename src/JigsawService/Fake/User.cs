using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace JigsawService.Fake
{
    internal sealed class User : IUser
    {
        private readonly ILogger<User> logger;

        public event Action<IRpcToken, byte[]> UploadJigsaw;

        public User(ILogger<User> logger)
        {
            this.logger = logger;
        }

        public void RaiseUploadJigsawEvent(string path)
        {
            var bytes = File.ReadAllBytes(path);
            UploadJigsaw.Invoke(null, bytes);
        }

        public IUser SendError(IRpcToken token, string message)
        {
            logger.LogInformation($"Error: {message}");
            return this;
        }

        public void SendTemplet(IRpcToken token, string id, string templet)
        {
            logger.LogDebug($"Reponse: {id} {templet}");
        }
    }
}
