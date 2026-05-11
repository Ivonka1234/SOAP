namespace SOAP.DTOs.TripLocation
{
    public class TripLocationResponseDto
    {
        public Guid LocationId { get; set; }

        public string LocationName { get; set; }

        public string Country { get; set; }

        public int Order { get; set; }

        public DateTime ScheduledStartTime { get; set; }

        public int VisitDurationHours { get; set; }

        public decimal EstimatedCost { get; set; }
    }
}
