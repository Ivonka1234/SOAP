using AutoMapper;
using SOAP.DTOs.TripLocation;
using SOAP.Models;
using SOAP.Repository;

namespace SOAP.Services
{
    public class TripLocationService : ITripLocationService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ITripLocationRepository _tripLocationRepository;
        private readonly IMapper _mapper;

        public TripLocationService(
            ITripRepository tripRepository,
            ILocationRepository locationRepository,
            ITripLocationRepository tripLocationRepository,
            IMapper mapper)
        {
            _tripRepository = tripRepository;
            _locationRepository = locationRepository;
            _tripLocationRepository = tripLocationRepository;
            _mapper = mapper;
        }

        public async Task<List<TripLocationResponseDto>> GetTripLocationsAsync(Guid tripId)
        {
            var locations = await _tripLocationRepository.GetByTripIdAsync(tripId);
            return _mapper.Map<List<TripLocationResponseDto>>(locations);
        }

        public async Task<bool> AddLocationToTripAsync(Guid tripId, AddLocationToTripDTO dto)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip == null) return false;

            var location = await _locationRepository.GetByIdAsync(dto.LocationId);
            if (location == null) return false;

            var existing = await _tripLocationRepository.GetByTripIdAsync(tripId);

            if (existing.Any(x => x.LocationId == dto.LocationId))
                return false;

            var tripLocation = new TripLocation
            {
                TripId = tripId,
                LocationId = dto.LocationId
            };

            await _tripLocationRepository.AddAsync(tripLocation);
            return true;
        }

        public async Task<bool> RemoveLocationFromTripAsync(Guid tripId, Guid locationId)
        {
            var existing = await _tripLocationRepository.GetByTripIdAsync(tripId);

            if (!existing.Any(x => x.LocationId == locationId))
                return false;

            await _tripLocationRepository.DeleteAsync(tripId, locationId);
            return true;
        }
    }
}