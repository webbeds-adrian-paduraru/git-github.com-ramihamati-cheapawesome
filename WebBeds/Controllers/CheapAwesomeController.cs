using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebBeds.Integration.CheapAwesome;
using WebBeds.ModelsApi;

namespace WebBeds.Controllers
{
    [ApiController]
    [Route("api/bargains")]
    public class CheapAwesomeController : ControllerBase
    {
        private readonly ILogger<CheapAwesomeController> _logger;
        private readonly IServiceCheapBeds _serviceCheapBeds;

        public CheapAwesomeController(
            ILogger<CheapAwesomeController> logger,
            IServiceCheapBeds serviceCheapBeds)
        {

            _logger = logger;
            this._serviceCheapBeds = serviceCheapBeds;
        }

        [HttpGet("findBargain")]
        public async Task<IActionResult> FindBargain([FromQuery] RequestFindBargain request)
        {
            var result = await _serviceCheapBeds.FindBargainAsync(request.DestinationId, request.Nights);

            return Ok(result);
        }
    }
}
