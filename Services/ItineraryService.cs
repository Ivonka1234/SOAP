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

        public async Task<Dictionary<int, List<ItineraryStopDto>>?> GenerateSmartItineraryAsync(
            Guid tripId,
            string userId)
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

            var totalDays = TripDateHelper.GetInclusiveCalendarDayCount(
                trip.StartDate,
                trip.EndDate);

            return DistributeLocations(ordered, totalDays);
        }

        private Dictionary<int, List<ItineraryStopDto>> DistributeLocations(
     List<TripLocation> locations,
     int totalDays)
        {
            var itinerary = new Dictionary<int, List<ItineraryStopDto>>();

            for (int day = 1; day <= totalDays; day++)
            {
                itinerary[day] = new List<ItineraryStopDto>();
            }

            if (locations.Count == 0)
                return itinerary;

            for (int day = 1; day <= totalDays; day++)
            {
                var location = locations[(day - 1) % locations.Count].Location;

                itinerary[day].Add(new ItineraryStopDto
                {
                    LocationId = location.Id,
                    LocationName = location.Name,
                    Country = location.Country,
                    Order = day,
                    EstimatedCost = location.EstimatedCost,
                    ScheduledStartTime = null
                });
            }

            return itinerary;
        }
    }
}