using System.Threading.Tasks;
using GatewayApi.Model;

namespace GatewayApi
{
    public interface IJigsawService
    {
        Task<UploadJigsawResponse> UploadJigsaw(UploadJigsawRequest request);
        Task<ChooseTempletResponse> ChooseTemplet(ChooseTempletRequest request);
        Task<ConfirmJigsawResponse> ConfirmJigsaw(ConfirmJigsawRequest request);
    }
}
