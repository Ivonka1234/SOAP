using SOAP.Models;

namespace SOAP.Repository
{
    public interface ITripLocationRepository
    {
        Task AddAsync(TripLocation tripLocation);
        Task<List<TripLocation>> GetByTripIdAsync(Guid tripId);
        Task SaveAsync();
    }
}
