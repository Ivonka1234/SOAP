using SOAP.DTOs.TripLocation;
using SOAP.Helpers;
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

        public async Task<Dictionary<int, List<ItineraryStopDto>>?> GenerateSmartItineraryAsync(Guid tripId, string userId)
        {
            if (!await _tripRepository.BelongsToUserAsync(tripId, userId))
                return null;

            var trip = await _tripRepository.GetByIdAsync(tripId);

            if (trip == null)
                throw new Exception("Trip not found");

            var tripLocations = await _tripLocationRepository.GetByTripIdAsync(tripId);

            var ordered = tripLocations
                .OrderByDescending(x => x.Location.Priority)
                .ThenBy(x => x.Location.EstimatedCost)
                .ToList();

            var filtered = FilterByBudget(ordered, trip.Budget);

            var totalDays = TripDateHelper.GetInclusiveCalendarDayCount(trip.StartDate, trip.EndDate);

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

        private Dictionary<int, List<ItineraryStopDto>> DistributeLocations(
            List<TripLocation> locations,
            int totalDays)
        {
            var itinerary = new Dictionary<int, List<ItineraryStopDto>>();

            for (int i = 1; i <= totalDays; i++)
                itinerary[i] = new List<ItineraryStopDto>();

            if (totalDays <= 0 || locations.Count == 0)
                return itinerary;

            var order = 1;
            var displayTime = DateTime.Today.AddHours(9);

            for (var i = 0; i < locations.Count; i++)
            {
                var day = (i % totalDays) + 1;
                var loc = locations[i].Location;

                itinerary[day].Add(new ItineraryStopDto
                {
                    LocationId = loc.Id,
                    LocationName = loc.Name,
                    Country = loc.Country,
                    Order = order++,
                    EstimatedCost = loc.EstimatedCost,
                    ScheduledStartTime = displayTime
                });
            }

            return itinerary;
        }
    }
}
