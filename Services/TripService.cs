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

        public async Task<IEnumerable<TripResponseDTO>> GetAllTripsAsync()
        {
            var trips = await _tripRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TripResponseDTO>>(trips);
        }

        public async Task<TripResponseDTO?> GetTripByIdAsync(Guid id)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null) return null;

            var dto = _mapper.Map<TripResponseDTO>(trip);
            var locations = await _tripLocationRepository.GetByTripIdAsync(id);

            dto.TotalEstimatedCost = locations.Sum(x => x.Location.EstimatedCost);
            dto.Locations = _mapper.Map<List<TripLocationResponseDto>>(locations);

            return dto;
        }

        public async Task<TripResponseDTO> CreateTripAsync(CreateTripDTO dto)
        {
            if (!ValidateTripDates(dto.StartDate, dto.EndDate))
                throw new Exception("Invalid trip dates");

            var trip = _mapper.Map<Models.Trip>(dto);
            trip.Id = Guid.NewGuid();

            await _tripRepository.AddAsync(trip);

            return _mapper.Map<TripResponseDTO>(trip);
        }

        public async Task<TripResponseDTO?> UpdateTripAsync(Guid id, UpdateTripDTO dto)
        {
            var existing = await _tripRepository.GetByIdAsync(id);
            if (existing == null) return null;

            if (!ValidateTripDates(dto.StartDate, dto.EndDate))
                throw new Exception("Invalid trip dates");

            _mapper.Map(dto, existing);

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

        public async Task<decimal> CalculateTotalCostAsync(Guid tripId)
        {
            var locations = await _tripLocationRepository.GetByTripIdAsync(tripId);
            return locations.Sum(x => x.Location.EstimatedCost);
        }

        public async Task<bool> CanAddLocationAsync(Guid tripId, decimal estimatedLocationCost)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip == null) return false;

            var currentCost = await CalculateTotalCostAsync(tripId);

            return currentCost + estimatedLocationCost <= trip.Budget;
        }

        public bool ValidateTripDates(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && startDate >= DateTime.UtcNow.Date;
        }

        public async Task<bool> IsTripOverBudget(Guid tripId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip == null) return false;

            var totalCost = await CalculateTotalCostAsync(tripId);

            return totalCost > trip.Budget;
        }

        public async Task<int> GetTripDurationDays(Guid tripId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip == null) return 0;

            return (trip.EndDate - trip.StartDate).Days + 1;
        }

        public async Task<bool> TripExists(Guid tripId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            return trip != null;
        }
    }
}