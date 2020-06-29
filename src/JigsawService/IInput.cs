using System;
using SixLabors.ImageSharp;

namespace JigsawService
{
    internal interface IInput
    {
        event Action<IRpcToken, byte[]> UploadJigsaw;
        event Action<IRpcToken, string, string> ChooseTemplet;

        IInput SendError(IRpcToken token, string message);
        void SendTemplet(IRpcToken token, string id, string templet);
        void SendPreview(IRpcToken token, string id, Image preview, int cost);
    }
}
