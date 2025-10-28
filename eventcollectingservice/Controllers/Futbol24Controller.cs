using eventcollectingservice.Models.Futbol24;
using eventcollectingservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace eventcollectingservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Futbol24Controller : ControllerBase
    {
        private readonly IFutbol24Service _futbol24Service;

        public Futbol24Controller(IFutbol24Service futbol24Service)
        {
            _futbol24Service = futbol24Service;
        }

        [HttpPost("events")]
        public async Task<IActionResult> PostEvent(Futbol24EventRequest request)
        {
            try
            {
                await _futbol24Service.ProcessFutbol24Event(request.Events);
                return Ok(new { message = "Futbol24 event processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
