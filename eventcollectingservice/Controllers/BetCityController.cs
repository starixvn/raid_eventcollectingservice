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

        [HttpPost("results/soccer")]
        public async Task<IActionResult> PostSoccerResult(SoccerResultRequest request)
        {
            try
            {
                await betCityService.ProcessSoccerResults(request.Results);
                return Ok(new { message = "BetCity soccer result processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("results/basketball")]
        public async Task<IActionResult> PostBasketballResult(BasketballResultRequest request)
        {
            try
            {
                await betCityService.ProcessBasketballResults(request.Results);
                return Ok(new { message = "BetCity basketball result processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("results/volleyball")]
        public async Task<IActionResult> PostVolleyballResult(VolleyballResultRequest request)
        {
            try
            {
                await betCityService.ProcessVolleyballResults(request.Results);
                return Ok(new { message = "BetCity volleyball result processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
