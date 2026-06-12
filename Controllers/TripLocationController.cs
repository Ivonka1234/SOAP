using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOAP.DTOs.TripLocation;
using SOAP.Extensions;
using SOAP.Services;

namespace SOAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TripLocationController : ControllerBase
    {
        private readonly ITripLocationService _tripLocationService;
        private readonly ITripService _tripService;

        public TripLocationController(
            ITripLocationService tripLocationService,
            ITripService tripService)
        {
            _tripLocationService = tripLocationService;
            _tripService = tripService;
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetByTrip(Guid tripId)
        {
            if (!await _tripService.UserOwnsTripAsync(tripId, User.GetUserId()))
                return NotFound();

            var result = await _tripLocationService.GetTripLocationsAsync(tripId, User.GetUserId());
            return Ok(result);
        }

        [HttpPost("{tripId}")]
        public async Task<IActionResult> Add(Guid tripId, [FromBody] AddLocationToTripDTO dto)
        {
            if (dto == null || dto.LocationId == Guid.Empty)
                return BadRequest("LocationId is required");

            var result = await _tripLocationService.AddLocationToTripAsync(tripId, dto, User.GetUserId());

            if (!result)
                return BadRequest("Trip or Location invalid OR already added");

            return NoContent();
        }

        [HttpDelete("{tripId}/{locationId}")]
        public async Task<IActionResult> Remove(Guid tripId, Guid locationId)
        {
            var result = await _tripLocationService.RemoveLocationFromTripAsync(tripId, locationId, User.GetUserId());

            if (!result)
                return NotFound("Relation not found");

            return NoContent();
        }
    }
}
