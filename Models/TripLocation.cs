namespace SOAP.Models
{
    public class TripLocation
    {
        public Guid Id { get; set; }

        public Guid TripId { get; set; }

        public Trip Trip { get; set; }

        public Guid LocationId { get; set; }

        public Location Location { get; set; }

        public int Order { get; set; }

        public DateTime ScheduledStartTime { get; set; }
    }
}
