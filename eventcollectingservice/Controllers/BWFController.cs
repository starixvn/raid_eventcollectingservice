using eventcollectingservice.Models.BWF;
using eventcollectingservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace eventcollectingservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BWFController : ControllerBase
    {
        private readonly IBWFService _bwfService;

        public BWFController(IBWFService bwfService)
        {
            _bwfService = bwfService;
        }

        [HttpPost("events")]
        public async Task<IActionResult> PostEven([FromBody] BWFMatchesRequest request)
        {
            try
            {
                if (request?.Events == null || !request.Events.Any())
                {
                    return BadRequest(new { error = "Events data is required" });
                }

                await _bwfService.ProcessBWFMatch(request.Events);

                return Ok(new 
                { 
                    message = "BWF events processed successfully",
                    eventsCount = request.Events.Count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
