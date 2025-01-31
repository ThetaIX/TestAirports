using Airports.Models;

namespace Airports.Interfaces
{
    public interface IAirportService
    {
        Task<Airport?> GetAirportCoordinatesAsync(string iataCode);
    }
}
