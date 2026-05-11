namespace SOAP.Models
{
    public class Location
    {
        //Model
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public decimal EstimatedCost { get; set; }

        public int VisitDurationHours { get; set; }

        public int Priority { get; set; }

        public ICollection<TripLocation> TripLocations { get; set; }
    }
}
