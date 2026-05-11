namespace SOAP.DTOs.Trip
{
    public class TripResponseDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal TotalEstimatedCost { get; set; }

        public List<TripLocationResponseDto> Locations { get; set; }
    }
}
