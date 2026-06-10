using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOAP.Extensions;
using SOAP.Services;

namespace SOAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItineraryController : ControllerBase
    {
        private readonly IItineraryService _itineraryService;

        public ItineraryController(IItineraryService itineraryService)
        {
            _itineraryService = itineraryService;
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> Generate(Guid tripId)
        {
            try
            {
                var itinerary = await _itineraryService.GenerateSmartItineraryAsync(tripId, User.GetUserId());

                if (itinerary == null)
                    return NotFound();

                if (!itinerary.Any())
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
