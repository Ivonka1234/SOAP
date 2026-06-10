using SOAP.DTOs.TripLocation;

namespace SOAP.Services
{
    public interface ITripLocationService
    {
        Task<List<TripLocationResponseDto>> GetTripLocationsAsync(Guid tripId, string userId);
        Task<bool> AddLocationToTripAsync(Guid tripId, AddLocationToTripDTO dto, string userId);
        Task<bool> RemoveLocationFromTripAsync(Guid tripId, Guid locationId, string userId);
    }
}
