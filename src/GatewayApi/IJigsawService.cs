using System.Threading.Tasks;
using GatewayApi.Model;

namespace GatewayApi
{
    public interface IJigsawService
    {
        Task<UploadJigsawResponse> UploadJigsaw(byte[] image);
    }
}
