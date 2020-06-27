using System;

namespace JigsawService
{
    internal interface IUser
    {
        event Action<IRpcToken, byte[]> UploadJigsaw;

        IUser SendError(IRpcToken token, string message);
        void SendTemplet(IRpcToken token, string id, string templet);
    }
}
