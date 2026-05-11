using SOAP.Models;

namespace SOAP.Repository
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllAsync();
        Task<Location?> GetByIdAsync(Guid id);
        Task AddAsync(Location location);
        Task UpdateAsync(Location entity);
        Task DeleteAsync(Guid id);
    }
}
