using Xunit;
using Moq;
using Airports.Services;
using Airports.Interfaces;
using Airports.Models;

namespace Airports.Tests
{
    public class DistanceServiceTests
    {
        private readonly DistanceService _distanceService;
        private readonly Mock<IAirportService> _mockGeoService;
        private readonly Mock<IDistanceCacheService> _mockDistanceCache;

        public DistanceServiceTests()
        {
            _mockGeoService = new Mock<IAirportService>();
            _mockDistanceCache = new Mock<IDistanceCacheService>();
            _distanceService = new DistanceService(_mockGeoService.Object, _mockDistanceCache.Object);
        }

        [Fact]
        public async Task CalculateDistanceAsync_ValidAirports_ReturnsCorrectDistance()
        {
            var airport1 = new Airport { Iata = "AMS", Location =  new Location { Lat = 52.309069, Lon = 4.763385 } }; // AMS (Amsterdam)
            var airport2 = new Airport { Iata = "JFK", Location = new Location { Lat = 40.641311, Lon = -73.778139 } }; // JFK (New York)

            _mockGeoService.Setup(s => s.GetAirportCoordinatesAsync("AMS"))
                .ReturnsAsync(airport1);
            _mockGeoService.Setup(s => s.GetAirportCoordinatesAsync("JFK"))
                .ReturnsAsync(airport2);

            var request = new DistRequest { FromIata = "AMS", ToIata = "JFK" };

            var result = await _distanceService.CalculateDistanceAsync(request);

            Assert.NotNull(result);
            Assert.InRange(result.DistanceKm, 5800, 6000); // (~5877 км)
        }

        [Fact]
        public async Task CalculateDistanceAsync_SameAirport_ReturnsZero()
        {
            var airport = new Airport { Iata = "AMS", Location = new Location { Lat = 52.309069, Lon = 4.763385 } }; // AMS

            _mockGeoService.Setup(s => s.GetAirportCoordinatesAsync("AMS"))
                .ReturnsAsync(airport);

            var request = new DistRequest { FromIata = "AMS", ToIata = "AMS" };

            var result = await _distanceService.CalculateDistanceAsync(request);

            Assert.NotNull(result);
            Assert.Equal(0, result.DistanceKm);
        }

        [Fact]
        public async Task CalculateDistanceAsync_InvalidAirport_ReturnsNull()
        {
            _mockGeoService.Setup(s => s.GetAirportCoordinatesAsync("INVALID"))
                .ReturnsAsync((Airport?)null);

            var request = new DistRequest { FromIata = "INVALID", ToIata = "JFK" };

            var result = await _distanceService.CalculateDistanceAsync(request);

            Assert.Null(result);
        }
    }
}
