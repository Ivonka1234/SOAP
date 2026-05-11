using SOAP.Models;

namespace SOAP.Services
{
    public interface IItineraryService
    {
        Task<Dictionary<int, List<Location>>> GenerateSmartItineraryAsync(Guid tripId);
    }
}
