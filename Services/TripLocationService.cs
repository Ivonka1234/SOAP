using SOAP.Models;
using SOAP.Repository;

namespace SOAP.Services
{
    public class TripLocationService : ITripLocationService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ITripLocationRepository _tripLocationRepository;

        public TripLocationService(
            ITripRepository tripRepository,
            ILocationRepository locationRepository,
            ITripLocationRepository tripLocationRepository)
        {
            _tripRepository = tripRepository;
            _locationRepository = locationRepository;
            _tripLocationRepository = tripLocationRepository;
        }

        public async Task<List<TripLocation>> GetTripLocationsAsync(Guid tripId)
        {
            return await _tripLocationRepository.GetByTripIdAsync(tripId);
        }

        public async Task<bool> AddLocationToTripAsync(Guid tripId, Guid locationId)
        {
            
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip == null)
                return false;

            
            var location = await _locationRepository.GetByIdAsync(locationId);
            if (location == null)
                return false;

            
            var existing = await _tripLocationRepository.GetByTripIdAsync(tripId);

            if (existing.Any(tl => tl.LocationId == locationId))
                return false;

            var tripLocation = new TripLocation
            {
                TripId = tripId,
                LocationId = locationId
            };

            await _tripLocationRepository.AddAsync(tripLocation);
            return true;
        }

        public async Task<bool> RemoveLocationFromTripAsync(Guid tripId, Guid locationId)
        {
            await _tripLocationRepository.DeleteAsync(tripId, locationId);
            return true;
        }
    }
}