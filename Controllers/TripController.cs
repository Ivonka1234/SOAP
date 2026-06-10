using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOAP.DTOs.Trip;
using SOAP.Extensions;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trips = await _tripService.GetAllTripsAsync(User.GetUserId());
            return Ok(trips);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var trip = await _tripService.GetTripByIdAsync(id, User.GetUserId());
            if (trip == null)
                return NotFound();

            return Ok(trip);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTripDTO dto)
        {
            try
            {
                var created = await _tripService.CreateTripAsync(dto, User.GetUserId());

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    created
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTripDTO dto)
        {
            try
            {
                var result = await _tripService.UpdateTripAsync(id, dto, User.GetUserId());

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _tripService.DeleteTripAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}/cost")]
        public async Task<IActionResult> GetTotalCost(Guid id)
        {
            if (!await _tripService.UserOwnsTripAsync(id, User.GetUserId()))
                return NotFound();

            var cost = await _tripService.CalculateTotalCostAsync(id, User.GetUserId());
            return Ok(cost);
        }

        [HttpGet("{id}/duration")]
        public async Task<IActionResult> GetDuration(Guid id)
        {
            if (!await _tripService.UserOwnsTripAsync(id, User.GetUserId()))
                return NotFound();

            var days = await _tripService.GetTripDurationDays(id, User.GetUserId());
            return Ok(days);
        }

        [HttpGet("{id}/overbudget")]
        public async Task<IActionResult> IsOverBudget(Guid id)
        {
            if (!await _tripService.UserOwnsTripAsync(id, User.GetUserId()))
                return NotFound();

            var result = await _tripService.IsTripOverBudget(id, User.GetUserId());
            return Ok(result);
        }
    }
}
