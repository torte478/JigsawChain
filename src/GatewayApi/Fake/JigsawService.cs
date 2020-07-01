using System.Threading.Tasks;
using GatewayApi.Model;

namespace GatewayApi.Fake
{
    internal sealed class JigsawService : IJigsawService
    {
        public Task<ChooseTempletResponse> ChooseTemplet(
                                    ChooseTempletRequest request)
        {
            return Task.Run(() => new ChooseTempletResponse
            {
                Id = "previewId123",
                Cost = 4815,
                Preview = new byte[] { 4, 8, 15, 16, 23, 42 },
            });
        }

        public Task<UploadJigsawResponse> UploadJigsaw(UploadJigsawRequest request)
        {
            return Task.Run(() => new UploadJigsawResponse
            {
                Id = "123",
                Templet = "{banana}",
            });
        }
    }
}
