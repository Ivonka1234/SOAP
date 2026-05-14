using AutoMapper;
using SOAP.DTOs.TripLocation;
using SOAP.Models;
using SOAP.Repository;

namespace SOAP.Services
{
    public class ItineraryService : IItineraryService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripLocationRepository _tripLocationRepository;
        private readonly IMapper _mapper;

        public ItineraryService(
            ITripRepository tripRepository,
            ITripLocationRepository tripLocationRepository,
            IMapper mapper)
        {
            _tripRepository = tripRepository;
            _tripLocationRepository = tripLocationRepository;
            _mapper = mapper;
        }

        public async Task<Dictionary<int, List<TripLocationResponseDto>>> GenerateSmartItineraryAsync(Guid tripId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);

            if (trip == null)
                throw new Exception("Trip not found");

            var tripLocations = await _tripLocationRepository.GetByTripIdAsync(tripId);

            var ordered = tripLocations
                .OrderByDescending(x => x.Location.Priority)
                .ThenBy(x => x.Location.EstimatedCost)
                .ToList();

            var filtered = FilterByBudget(ordered, trip.Budget);

            var totalDays = (trip.EndDate - trip.StartDate).Days + 1;

            return DistributeLocations(filtered, totalDays);
        }

        private List<TripLocation> FilterByBudget(List<TripLocation> locations, decimal budget)
        {
            var result = new List<TripLocation>();
            decimal total = 0;

            foreach (var item in locations)
            {
                if (total + item.Location.EstimatedCost <= budget)
                {
                    result.Add(item);
                    total += item.Location.EstimatedCost;
                }
            }

            return result;
        }

        private Dictionary<int, List<TripLocationResponseDto>> DistributeLocations(
            List<TripLocation> locations,
            int totalDays)
        {
            var itinerary = new Dictionary<int, List<TripLocationResponseDto>>();

            for (int i = 1; i <= totalDays; i++)
                itinerary[i] = new List<TripLocationResponseDto>();

            int currentDay = 1;
            int currentHours = 0;
            int maxHoursPerDay = 8;

            foreach (var location in locations)
            {
                if (currentHours + location.Location.VisitDurationHours > maxHoursPerDay)
                {
                    currentDay++;
                    currentHours = 0;

                    if (currentDay > totalDays)
                        break;
                }

                var dto = _mapper.Map<TripLocationResponseDto>(location);

                itinerary[currentDay].Add(dto);
                currentHours += location.Location.VisitDurationHours;
            }

            return itinerary;
        }
    }
}