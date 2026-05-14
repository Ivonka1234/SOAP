using SOAP.DTOs.TripLocation;
using SOAP.Models;

namespace SOAP.Services
{
    public interface ITripLocationService
    {
        Task<List<TripLocationResponseDto>> GetTripLocationsAsync(Guid tripId);
        Task<bool> AddLocationToTripAsync(Guid tripId, AddLocationToTripDTO dto);
        Task<bool> RemoveLocationFromTripAsync(Guid tripId, Guid locationId);
    }
}