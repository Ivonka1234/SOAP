using SOAP.Models;

namespace SOAP.Repository
{
    public interface ITripLocationRepository
    {
        Task<List<TripLocation>> GetByTripIdAsync(Guid tripId);
        Task AddAsync(TripLocation tripLocation);
        Task DeleteAsync(Guid tripId, Guid locationId);
    }
}