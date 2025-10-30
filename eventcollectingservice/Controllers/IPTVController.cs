using eventcollectingservice.Models.IPTV;
using eventcollectingservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace eventcollectingservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IPTVController : ControllerBase
    {
        private readonly IIPTVService _iptvService;

        public IPTVController(IIPTVService iptvService)
        {
            _iptvService = iptvService;
        }

        [HttpPost("events")]
        public async Task<IActionResult> PostEvent(IPTVEventRequest request)
        {
            try
            {
                await _iptvService.ProcessIPTVEvent(request.Events);
                return Ok(new { message = "IPTV event processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}