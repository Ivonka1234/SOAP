using SOAP.Models;
using SOAP.Repository;

namespace SOAP.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            
            var existing = await _userRepository.GetByEmailAsync(user.Email);

            if (existing != null)
                throw new Exception("User already exists");

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<bool> UpdateUserAsync(Guid id, User updatedUser)
        {
            var existing = await _userRepository.GetByEmailAsync(updatedUser.Email);

            if (existing == null)
                return false;

            existing.FullName = updatedUser.FullName;
            existing.Email = updatedUser.Email;

            await _userRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
            return true;
        }
    }
}