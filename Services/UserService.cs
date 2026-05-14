using AutoMapper;
using SOAP.DTOs.User;
using SOAP.Repository;

namespace SOAP.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponseDTO?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return null;

            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO?> UpdateUserAsync(Guid id, UpdateUserDTO dto)
        {
            var existing = await _userRepository.GetByIdAsync(id);

            if (existing == null)
                return null;

            existing.FullName = dto.FullName;
            existing.Email = dto.Email;

            await _userRepository.UpdateAsync(existing);

            return _mapper.Map<UserResponseDTO>(existing);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var existing = await _userRepository.GetByIdAsync(id);

            if (existing == null)
                return false;

            await _userRepository.DeleteAsync(id);
            return true;
        }
    }
}