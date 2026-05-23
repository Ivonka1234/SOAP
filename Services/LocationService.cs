using AutoMapper;
using SOAP.DTOs.Location;
using SOAP.Models;
using SOAP.Repository;

namespace SOAP.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LocationResponseDTO>> GetAllAsync()
        {
            var locations = await _locationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LocationResponseDTO>>(locations);
        }

        public async Task<LocationResponseDTO?> GetByIdAsync(Guid id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            return location == null ? null : _mapper.Map<LocationResponseDTO>(location);
        }

        public async Task<LocationResponseDTO> CreateAsync(CreateLocationDTO dto)
        {
            var location = _mapper.Map<Location>(dto);
            location.Id = Guid.NewGuid();

            await _locationRepository.AddAsync(location);

            return _mapper.Map<LocationResponseDTO>(location);
        }

        public async Task<LocationResponseDTO?> UpdateAsync(Guid id, UpdateLocationDTO dto)
        {
            var existing = await _locationRepository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);

            await _locationRepository.UpdateAsync(existing);

            return _mapper.Map<LocationResponseDTO>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _locationRepository.GetByIdAsync(id);
            if (existing == null) return false;

            await _locationRepository.DeleteAsync(id);
            return true;
        }
    }
}