using SOAP.DTOs.TripLocation;

namespace SOAP.Services
{
    public interface IItineraryService
    {
        Task<Dictionary<int, List<ItineraryStopDto>>?> GenerateSmartItineraryAsync(Guid tripId, string userId);
    }
}
