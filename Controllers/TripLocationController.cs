using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOAP.Services;

namespace SOAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class TripLocationController : ControllerBase
    {
        private readonly ITripLocationService _tripLocationService;

        public TripLocationController(ITripLocationService tripLocationService)
        {
            _tripLocationService = tripLocationService;
        }

        //  GET: api/triplocation/{tripId}
        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetByTrip(Guid tripId)
        {
            var result = await _tripLocationService.GetTripLocationsAsync(tripId);
            return Ok(result);
        }

        //  POST: api/triplocation
        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] Guid tripId, [FromQuery] Guid locationId)
        {
            var result = await _tripLocationService.AddLocationToTripAsync(tripId, locationId);

            if (!result)
                return BadRequest("Trip or Location invalid OR already added");

            return Ok("Location added to trip");
        }

        //  DELETE: api/triplocation/{tripId}/{locationId}
        [HttpDelete("{tripId}/{locationId}")]
        public async Task<IActionResult> Remove(Guid tripId, Guid locationId)
        {
            var result = await _tripLocationService.RemoveLocationFromTripAsync(tripId, locationId);

            if (!result)
                return NotFound("Relation not found");

            return NoContent();
        }
    }
}