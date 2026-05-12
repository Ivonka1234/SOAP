using Microsoft.AspNetCore.Mvc;
using SOAP.Services;

namespace SOAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        private readonly IItineraryService _itineraryService;

        public ItineraryController(IItineraryService itineraryService)
        {
            _itineraryService = itineraryService;
        }

        // GET: api/itinerary/{tripId}
        [HttpGet("{tripId}")]
        public async Task<IActionResult> Generate(Guid tripId)
        {
            try
            {
                var itinerary = await _itineraryService.GenerateSmartItineraryAsync(tripId);
                return Ok(itinerary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}