using SOAP.Models;

namespace SOAP.Repository
{
    public interface ITripRepository
    {
        Task<List<Trip>> GetAllAsync();
        Task<List<Trip>> GetByUserIdAsync(string userId);
        Task<Trip?> GetByIdAsync(Guid id);
        Task<bool> BelongsToUserAsync(Guid tripId, string userId);
        Task AddAsync(Trip trip);
        Task UpdateAsync(Trip trip);
        Task DeleteAsync(Guid id);
    }
}
