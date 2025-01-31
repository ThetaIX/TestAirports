using Airports.Interfaces;
using Airports.Models;
using Airports.Utils;

namespace Airports.Services
{
    public class DistanceService : IDistanceService
    {
        private readonly IAirportService _airportService;
        private readonly IDistanceCacheService _distanceCache;

        public DistanceService(IAirportService airportService, IDistanceCacheService distanceCache)
        {
            _airportService = airportService;
            _distanceCache = distanceCache;
        }

        public async Task<DistResponse> CalculateDistanceAsync(DistRequest request)
        {
            var cachedResponse = await _distanceCache.GetCachedDistanceAsync(request.FromIata, request.ToIata);
            if (cachedResponse != null)
            {
                return cachedResponse;
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

            DistResponse distResponse = new DistResponse
            {
                From = fromAirport.Iata,
                To = toAirport.Iata,
                DistanceKm = distance,
                Message = "The distance has been successfully calculated."
            };

            await _distanceCache.SaveDistanceAsync(distResponse);

            return distResponse;
        }
    }
}
