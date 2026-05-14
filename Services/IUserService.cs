using SOAP.DTOs.User;
using SOAP.Models;

namespace SOAP.Services
{
    public interface IUserService
    {
        Task<UserResponseDTO?> GetByEmailAsync(string email);
        Task<UserResponseDTO?> UpdateUserAsync(Guid id, UpdateUserDTO dto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}