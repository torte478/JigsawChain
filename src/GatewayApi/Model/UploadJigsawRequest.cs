using Microsoft.AspNetCore.Http;

namespace GatewayApi.Model
{
    public class UploadJigsawRequest
    {
        public IFormFile Image { get; set; }
    }
}
