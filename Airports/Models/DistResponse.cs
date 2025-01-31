namespace Airports.Models
{
    public class DistResponse
    {
        public string From { get; set; }
        public string To { get; set; }
        public double DistanceKm { get; set; }
        public string Message { get; set; }
    }
}
