using Airports.Interfaces;
using Airports.Models;
using Airports.Utils;

namespace Airports.Services
{
    public class DistanceService : IDistanceService
    {
        private readonly IAirportService _airportService;

        public DistanceService(IAirportService airportService)
        {
            _airportService = airportService;
        }

        public async Task<DistResponse> CalculateDistanceAsync(DistRequest request)
        {
            if (request.FromIata.ToUpper() == request.ToIata.ToUpper())
            {
                return new DistResponse
                {
                    From = request.FromIata,
                    To = request.ToIata,
                    DistanceKm = 0,
                    Message = "Both airports are the same, the distance is 0."
                };
            }

            var fromAirport = await _airportService.GetAirportCoordinatesAsync(request.FromIata);
            var toAirport = await _airportService.GetAirportCoordinatesAsync(request.ToIata);

            if (fromAirport == null || toAirport == null)
            {
                return null;
            }

            var distance = DistanceCalculator.CalculateHaversineDistance(
                fromAirport.Location.Lat, fromAirport.Location.Lon,
                toAirport.Location.Lat, toAirport.Location.Lon
            );

            return new DistResponse
            {
                From = fromAirport.Iata,
                To = toAirport.Iata,
                DistanceKm = distance,
                Message = "The distance has been successfully calculated."
            };
        }
    }
}
