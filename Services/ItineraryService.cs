using SOAP.Models;
using SOAP.Repository;

namespace SOAP.Services
{
    public class ItineraryService : IItineraryService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripLocationRepository _tripLocationRepository;

        public ItineraryService(
            ITripRepository tripRepository,
            ITripLocationRepository tripLocationRepository)
        {
            _tripRepository = tripRepository;
            _tripLocationRepository = tripLocationRepository;
        }

        public async Task<Dictionary<int, List<Location>>> GenerateSmartItineraryAsync(Guid tripId)
        {
            var trip = await GetTripOrThrow(tripId);

            var locations = await GetSortedLocations(tripId);

            locations = FilterByBudget(locations, trip.Budget);

            int totalDays = CalculateTripDays(trip);

            return DistributeLocations(locations, totalDays);
        }

    

        private async Task<Trip> GetTripOrThrow(Guid tripId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);

            if (trip == null)
                throw new Exception("Trip not found");

            return trip;
        }

        private async Task<List<Location>> GetSortedLocations(Guid tripId)
        {
            var tripLocations = await _tripLocationRepository.GetByTripIdAsync(tripId);

            return tripLocations
                .Select(tl => tl.Location)
                .OrderByDescending(l => l.Priority)
                .ThenBy(l => l.EstimatedCost)      
                .ToList();
        }

        private List<Location> FilterByBudget(List<Location> locations, decimal budget)
        {
            var result = new List<Location>();
            decimal total = 0;

            foreach (var loc in locations)
            {
                if (total + loc.EstimatedCost <= budget)
                {
                    result.Add(loc);
                    total += loc.EstimatedCost;
                }
            }

            return result;
        }

        private int CalculateTripDays(Trip trip)
        {
            return (trip.EndDate - trip.StartDate).Days + 1;
        }

        private Dictionary<int, List<Location>> DistributeLocations(List<Location> locations, int totalDays)
        {
            var itinerary = new Dictionary<int, List<Location>>();

            for (int i = 1; i <= totalDays; i++)
                itinerary[i] = new List<Location>();

            int currentDay = 1;
            int currentHours = 0;
            int maxHoursPerDay = 8;

            foreach (var location in locations)
            {
              
                if (currentHours + location.VisitDurationHours > maxHoursPerDay)
                {
                    currentDay++;
                    currentHours = 0;

                    if (currentDay > totalDays)
                        break;
                }

                itinerary[currentDay].Add(location);
                currentHours += location.VisitDurationHours;
            }

            return itinerary;
        }
    }
}