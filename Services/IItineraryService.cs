using SOAP.DTOs.TripLocation;
using SOAP.Models;

namespace SOAP.Services
{
    public interface IItineraryService
    {
        Task<Dictionary<int, List<TripLocationResponseDto>>> GenerateSmartItineraryAsync(Guid tripId);
    }
}
