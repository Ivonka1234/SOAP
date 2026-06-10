using SOAP.DTOs.Trip;

namespace SOAP.Services
{
    public interface ITripService
    {
        Task<IEnumerable<TripResponseDTO>> GetAllTripsAsync(string userId);
        Task<TripResponseDTO?> GetTripByIdAsync(Guid id, string userId);
        Task<TripResponseDTO> CreateTripAsync(CreateTripDTO dto, string userId);
        Task<TripResponseDTO?> UpdateTripAsync(Guid id, UpdateTripDTO dto, string userId);
        Task<bool> DeleteTripAsync(Guid id);
        Task<decimal> CalculateTotalCostAsync(Guid tripId, string userId);
        Task<bool> CanAddLocationAsync(Guid tripId, decimal estimatedLocationCost, string userId);
        Task<bool> IsTripOverBudget(Guid tripId, string userId);
        Task<int> GetTripDurationDays(Guid tripId, string userId);
        Task<bool> UserOwnsTripAsync(Guid tripId, string userId);
    }
}
