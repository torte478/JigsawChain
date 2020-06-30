using System;
using SixLabors.ImageSharp;

namespace JigsawService
{
    internal interface IInput
    {
        event Action<IRpcToken, byte[]> UploadJigsaw;
        event Action<IRpcToken, string, string> ChooseTemplet;
        event Action<IRpcToken, string> ConfirmJigsaw;

        IInput SendTemplet(IRpcToken token, string id, string templet);
        IInput SendPreview(IRpcToken token, string id, Image preview, int cost);
        IInput SendConfirmation(IRpcToken token, string id); 

        IInput SendError(IRpcToken token, string message);
    }
}
