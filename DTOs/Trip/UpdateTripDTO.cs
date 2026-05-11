namespace SOAP.DTOs.Trip
{
    public class UpdateTripDTO
    {
        public string Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
