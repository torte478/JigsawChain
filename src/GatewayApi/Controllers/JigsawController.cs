using Microsoft.AspNetCore.Mvc;

namespace GatewayApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JigsawController : ControllerBase
    {
        [HttpGet("{number}")]
        public string Get(int number)
        {
            return $"Hello, world! = {number}";
        }
    }
}
