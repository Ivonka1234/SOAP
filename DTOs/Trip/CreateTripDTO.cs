namespace SOAP.DTOs.Trip
{
    public class CreateTripDTO
    {
        public string Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
