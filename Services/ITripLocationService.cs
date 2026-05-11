using SOAP.Models;

namespace SOAP.Services
{
    public interface ITripLocationService
    {
        Task<List<TripLocation>> GetTripLocationsAsync(Guid tripId);
        Task<bool> AddLocationToTripAsync(Guid tripId, Guid locationId);
        Task<bool> RemoveLocationFromTripAsync(Guid tripId, Guid locationId);
    }
}