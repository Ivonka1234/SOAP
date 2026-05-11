using SOAP.Models;

namespace SOAP.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User entity);

        Task DeleteAsync(Guid id);
    }
}
