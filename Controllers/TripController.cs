using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOAP.DTOs.Trip;
using SOAP.Services;

namespace SOAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        // GET: api/trip
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trips = await _tripService.GetAllTripsAsync();
            return Ok(trips); // should be List<TripResponseDTO>
        }

        // GET: api/trip/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null)
                return NotFound();

            return Ok(trip); // TripResponseDTO
        }

        // POST: api/trip
        [HttpPost]
        public async Task<IActionResult> Create(CreateTripDTO dto)
        {
            var created = await _tripService.CreateTripAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );
        }

        // PUT: api/trip/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTripDTO dto)
        {
            var result = await _tripService.UpdateTripAsync(id, dto);

            if (result==null)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/trip/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _tripService.DeleteTripAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/trip/{id}/cost
        [HttpGet("{id}/cost")]
        public async Task<IActionResult> GetTotalCost(Guid id)
        {
            var cost = await _tripService.CalculateTotalCostAsync(id);
            return Ok(cost);
        }

        // GET: api/trip/{id}/duration
        [HttpGet("{id}/duration")]
        public async Task<IActionResult> GetDuration(Guid id)
        {
            var days = await _tripService.GetTripDurationDays(id);
            return Ok(days);
        }

        // GET: api/trip/{id}/overbudget
        [HttpGet("{id}/overbudget")]
        public async Task<IActionResult> IsOverBudget(Guid id)
        {
            var result = await _tripService.IsTripOverBudget(id);
            return Ok(result);
        }
    }
}