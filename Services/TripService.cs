using SOAP.Models;
using SOAP.Repository;

namespace SOAP.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripLocationRepository _tripLocationRepository;

        public TripService( ITripRepository tripRepository,ITripLocationRepository tripLocationRepository)
        {
            _tripRepository = tripRepository;
            _tripLocationRepository = tripLocationRepository;
        }

        public async Task<List<Trip>> GetAllTripsAsync()
        {
            return await _tripRepository.GetAllAsync();
        }

        public async Task<Trip?> GetTripByIdAsync(Guid id)
        {
            return await _tripRepository.GetByIdAsync(id);
        }

        public async Task<Trip> CreateTripAsync(Trip trip)
        {
            if (!ValidateTripDates(trip.StartDate, trip.EndDate))
                throw new Exception("Invalid trip dates");

            await _tripRepository.AddAsync(trip);
            return trip;
        }

        public async Task<bool> UpdateTripAsync(Guid id, Trip updatedTrip)
        {
            var existing = await _tripRepository.GetByIdAsync(id);

            if (existing == null)
                return false;

            if (!ValidateTripDates(updatedTrip.StartDate, updatedTrip.EndDate))
                return false;

            existing.Name = updatedTrip.Name;
            existing.Budget = updatedTrip.Budget;
            existing.StartDate = updatedTrip.StartDate;
            existing.EndDate = updatedTrip.EndDate;

            await _tripRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteTripAsync(Guid id)
        {
            var existing = await _tripRepository.GetByIdAsync(id);

            if (existing == null)
                return false;

            await _tripRepository.DeleteAsync(id);
            return true;
        }

        public async Task<decimal> CalculateTotalCostAsync(Guid tripId)
        {
            var tripLocations = await _tripLocationRepository.GetByTripIdAsync(tripId);

            return tripLocations.Sum(tl => tl.Location.EstimatedCost);
        }

        public async Task<bool> CanAddLocationAsync(Guid tripId, decimal estematedLocationCost)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);

            if (trip == null)
                return false;

            var currentCost = await CalculateTotalCostAsync(tripId);

            return (currentCost + estematedLocationCost) <= trip.Budget;
        }


        public bool ValidateTripDates(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && startDate >= DateTime.UtcNow.Date;
        }

        public async Task<int> GetTripDurationDays(Guid tripId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);

            if (trip == null)
                return 0;

            return (trip.EndDate - trip.StartDate).Days+1;
        }

        public async Task<bool> IsTripOverBudget(Guid tripId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);

            if (trip == null)
                return false;

            var totalCost = await CalculateTotalCostAsync(tripId);

            return totalCost > trip.Budget;
        }
        public async Task<bool> TripExists(Guid tripId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            return trip != null;
        }
    }
}
