namespace SOAP.DTOs.TripLocation
{
    public class ItineraryStopDto
    {
        public Guid LocationId { get; set; }

        public string LocationName { get; set; }

        public string Country { get; set; }

        public int Order { get; set; }

        public decimal EstimatedCost { get; set; }

        /// <summary>Fixed display time (9:00 AM) for the assigned day — not hour-stacked scheduling.</summary>
        public DateTime? ScheduledStartTime { get; set; }
    }
}
