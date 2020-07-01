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
        [Route("[action]")]
        public async Task<ActionResult<UploadJigsawResponse>> UploadJigsaw(
            [FromForm] UploadJigsawRequest request)
        {
            logger.LogInformation(
                $"Request (upload jigsaw): {request.Image.Length}");

            var response = await jigsawService.UploadJigsaw(request);

            logger.LogInformation($"Response: {response.Id}");
            return response;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ChooseTempletResponse>> ChooseTemplet(
                                                    ChooseTempletRequest request)
        {
            logger.LogInformation(
                $"Request (choose templet): {request.Id}");

            var response = await jigsawService.ChooseTemplet(request);

            logger.LogInformation($"Response: {response.Id}");
            return response;
        }
    }
}
