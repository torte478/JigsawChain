using System.Threading.Tasks;
using GatewayApi.Model;

namespace GatewayApi.Fake
{
    internal sealed class JigsawService : IJigsawService
    {
        public Task<UploadJigsawResponse> UploadJigsaw(byte[] image)
        {
            return Task.Run(() => new UploadJigsawResponse
            {
                Id = "123",
                Templet = "{banana}",
            });
        }
    }
}
