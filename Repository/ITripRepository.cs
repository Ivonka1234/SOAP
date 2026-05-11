using SOAP.Models;

namespace SOAP.Repository
{
    public interface ITripRepository
    {

        Task<List<Trip>> GetAllAsync();
        Task<Trip> GetByIdAsync(Guid id);
        Task AddAsync(Trip trip);
        Task UpdateAsync(Trip trip);
        Task DeleteAsync(Guid id);


    }
}
