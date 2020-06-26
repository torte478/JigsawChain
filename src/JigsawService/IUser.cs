using System;

namespace JigsawService
{
    internal interface IUser
    {
        event Action<IRpcToken, byte[]> UploadJigsaw;
    }
}
