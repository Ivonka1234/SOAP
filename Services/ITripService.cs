using SOAP.DTOs.Trip;
using SOAP.Models;

namespace SOAP.Services
{
    public interface ITripService
    {
        Task<IEnumerable<TripResponseDTO>> GetAllTripsAsync();
        Task<TripResponseDTO?> GetTripByIdAsync(Guid id);
        Task<TripResponseDTO> CreateTripAsync(CreateTripDTO dto);
        Task<TripResponseDTO?> UpdateTripAsync(Guid id, UpdateTripDTO dto);
        Task<bool> DeleteTripAsync(Guid id);
        Task<decimal> CalculateTotalCostAsync(Guid tripId);
        Task<bool> CanAddLocationAsync(Guid tripId, decimal estimatedLocationCost);
        Task<bool> IsTripOverBudget(Guid tripId);
        Task<int> GetTripDurationDays(Guid tripId);
        Task<bool> TripExists(Guid tripId);

    }
}
