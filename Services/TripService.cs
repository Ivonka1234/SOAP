using AutoMapper;
using SOAP.DTOs.Trip;
using SOAP.DTOs.TripLocation;
using SOAP.Repository;

namespace SOAP.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripLocationRepository _tripLocationRepository;
        private readonly IMapper _mapper;

        public TripService(
            ITripRepository tripRepository,
            ITripLocationRepository tripLocationRepository,
            IMapper mapper)
        {
            _tripRepository = tripRepository;
            _tripLocationRepository = tripLocationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TripResponseDTO>> GetAllTripsAsync(string userId)
        {
            var trips = await _tripRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<TripResponseDTO>>(trips);
        }

        public async Task<TripResponseDTO?> GetTripByIdAsync(Guid id, string userId)
        {
            if (!await UserOwnsTripAsync(id, userId))
                return null;

            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null) return null;

            var dto = _mapper.Map<TripResponseDTO>(trip);
            var locations = await _tripLocationRepository.GetByTripIdAsync(id);

            dto.TotalEstimatedCost = locations.Sum(x => x.Location.EstimatedCost);
            dto.Locations = _mapper.Map<List<TripLocationResponseDto>>(locations);

            return dto;
        }

        public async Task<TripResponseDTO> CreateTripAsync(CreateTripDTO dto, string userId)
        {
            dto.StartDate = NormalizeToUtcDate(dto.StartDate);
            dto.EndDate = NormalizeToUtcDate(dto.EndDate);

            if (!ValidateTripDates(dto.StartDate, dto.EndDate))
                throw new Exception("Invalid trip dates. Start must be today or later and not after end date.");

            var trip = _mapper.Map<Models.Trip>(dto);
            trip.Id = Guid.NewGuid();
            trip.UserId = userId;

            await _tripRepository.AddAsync(trip);

            var created = _mapper.Map<TripResponseDTO>(trip);
            created.Locations = new List<TripLocationResponseDto>();
            return created;
        }

        public async Task<TripResponseDTO?> UpdateTripAsync(Guid id, UpdateTripDTO dto, string userId)
        {
            if (!await UserOwnsTripAsync(id, userId))
                return null;

            var existing = await _tripRepository.GetByIdAsync(id);
            if (existing == null) return null;

            dto.StartDate = NormalizeToUtcDate(dto.StartDate);
            dto.EndDate = NormalizeToUtcDate(dto.EndDate);

            if (!ValidateTripDates(dto.StartDate, dto.EndDate))
                throw new Exception("Invalid trip dates. Start must be today or later and not after end date.");

            var ownerId = existing.UserId;
            _mapper.Map(dto, existing);
            existing.UserId = ownerId;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;

            await _tripRepository.UpdateAsync(existing);

            return _mapper.Map<TripResponseDTO>(existing);
        }

        public async Task<bool> DeleteTripAsync(Guid id)
        {
            var existing = await _tripRepository.GetByIdAsync(id);
            if (existing == null) return false;

            await _tripRepository.DeleteAsync(id);
            return true;
        }

        public async Task<decimal> CalculateTotalCostAsync(Guid tripId, string userId)
        {
            if (!await UserOwnsTripAsync(tripId, userId))
                return 0;

            var locations = await _tripLocationRepository.GetByTripIdAsync(tripId);
            return locations.Sum(x => x.Location.EstimatedCost);
        }

        public async Task<bool> CanAddLocationAsync(Guid tripId, decimal estimatedLocationCost, string userId)
        {
            if (!await UserOwnsTripAsync(tripId, userId))
                return false;

            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip == null) return false;

            var currentCost = await CalculateTotalCostAsync(tripId, userId);

            return currentCost + estimatedLocationCost <= trip.Budget;
        }

        public bool ValidateTripDates(DateTime startDate, DateTime endDate)
        {
            var start = startDate.Date;
            var end = endDate.Date;
            return start <= end && start >= DateTime.UtcNow.Date;
        }

        private static DateTime NormalizeToUtcDate(DateTime date) =>
            DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);

        public async Task<bool> IsTripOverBudget(Guid tripId, string userId)
        {
            if (!await UserOwnsTripAsync(tripId, userId))
                return false;

            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip == null) return false;

            var totalCost = await CalculateTotalCostAsync(tripId, userId);

            return totalCost > trip.Budget;
        }

        public async Task<int> GetTripDurationDays(Guid tripId, string userId)
        {
            if (!await UserOwnsTripAsync(tripId, userId))
                return 0;

            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip == null) return 0;

            return (trip.EndDate - trip.StartDate).Days + 1;
        }

        public Task<bool> UserOwnsTripAsync(Guid tripId, string userId) =>
            _tripRepository.BelongsToUserAsync(tripId, userId);
    }
}
