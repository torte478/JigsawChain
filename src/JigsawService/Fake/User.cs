using System;

namespace JigsawService.Fake
{
    internal sealed class User : IUser
    {
        public event Action<IRpcToken, byte[]> UploadJigsaw;

        public void RaiseUploadJigsawEvent(IRpcToken token, byte[] bytes)
        {
            UploadJigsaw.Invoke(null, bytes);
        }
    }
}
