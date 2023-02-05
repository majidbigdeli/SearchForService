using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchForApi.Services;

namespace SearchForApi.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("text/plain")]
    [Route("metrics")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class MetricsController : ControllerBase
    {
        private readonly IMetricService _metricService;

        public MetricsController(IMetricService metricService)
        {
            _metricService = metricService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _metricService.Get();
            return Ok(result);
        }
    }
}