namespace Airports.Models
{
    public class Airport
    {
        public string Iata { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }

    }

    public class Location
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }
}
