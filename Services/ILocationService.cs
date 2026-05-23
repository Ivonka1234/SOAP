using SOAP.DTOs.Location;

namespace SOAP.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationResponseDTO>> GetAllAsync();
        Task<LocationResponseDTO?> GetByIdAsync(Guid id);
        Task<LocationResponseDTO> CreateAsync(CreateLocationDTO dto);
        Task<LocationResponseDTO?> UpdateAsync(Guid id, UpdateLocationDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}