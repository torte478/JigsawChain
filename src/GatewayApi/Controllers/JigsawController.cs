using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GatewayApi.Model;

namespace GatewayApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JigsawController : ControllerBase
    {
        private readonly ILogger<JigsawController> logger;
        private readonly IJigsawService jigsawService;

        public JigsawController(
                        ILogger<JigsawController> logger,
                        IJigsawService jigsawService)
        {
            this.logger = logger;
            this.jigsawService = jigsawService;
        }

        [HttpGet("{number}")]
        public string Get(int number)
        {
            return $"Hello, world! = {number}";
        }

        [HttpPost]
        public async Task<ActionResult<UploadJigsawResponse>> UploadJigsaw(
            [FromForm] UploadJigsawRequest request)
        {
            logger.LogInformation(
                $"Request (upload jigsaw): {request.Image.Length}");

            using var memory = new MemoryStream();
            request.Image.CopyTo(memory);
            var image = memory.ToArray();

            var response = await jigsawService.UploadJigsaw(image);

            logger.LogInformation($"Response: {response.Id}");
            return response;
        }
    }
}
