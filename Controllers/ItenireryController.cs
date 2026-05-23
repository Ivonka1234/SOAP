using Microsoft.AspNetCore.Authorization;
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

                if (itinerary == null || !itinerary.Any())
                    return NotFound("No itinerary generated");

                return Ok(itinerary);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}