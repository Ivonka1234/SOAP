namespace SOAP.Models
{
    public class Trip
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }

        public ICollection<TripLocation> TripLocations { get; set; }
    }
}
