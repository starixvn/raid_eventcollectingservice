using eventcollectingservice.Models.BetCity;
using eventcollectingservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace eventcollectingservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BetCityController(IBetCityService betCityService) : ControllerBase
    {
        [HttpPost("events")]
        public async Task<IActionResult> PostEvent(BetCityEventsRequestDto betCityEventData)
        {
            try
            {
                if (betCityEventData.Events != null)
                    await betCityService.ProcessBetCityEvent(betCityEventData.Events);
                return Ok(new { message = "BetCity event processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("results")]
        public async Task<IActionResult> PostResult(BetCityResultRequest request)
        {
            try
            {
                await betCityService.ProcessBetCityResults(request.Results);
                return Ok(new { message = "BetCity result processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
