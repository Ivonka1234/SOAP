using SOAP.Models;

namespace SOAP.Services
{
    public interface ITripService
    {
        Task<List<Trip>> GetAllTripsAsync();
        Task<Trip?> GetTripByIdAsync(Guid id);
        Task<Trip> CreateTripAsync(Trip trip);
        Task<bool> UpdateTripAsync(Guid id, Trip updatedTrip);
        Task<bool> DeleteTripAsync(Guid id);
        Task<decimal> CalculateTotalCostAsync(Guid tripId);
        Task<bool> CanAddLocationAsync(Guid tripId, decimal locationCost);
        bool ValidateTripDates(DateTime startDate, DateTime endDate);
        Task<bool> IsTripOverBudget(Guid tripId);
        Task<int> GetTripDurationDays(Guid tripId);
        Task<bool> TripExists(Guid tripId);

    }
}
