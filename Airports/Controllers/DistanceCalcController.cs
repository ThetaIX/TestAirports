using Airports.Services;
using Airports.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Airports.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistanceCalcController : ControllerBase
    {
        private readonly AirportService _airportService;

        public DistanceCalcController(AirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDistance(string iata1, string iata2)
        {
            if (string.IsNullOrEmpty(iata1) || string.IsNullOrEmpty(iata2))
            {
                return BadRequest("IATA-коды не должны быть пустыми.");
            }

            var airport1 = await _airportService.GetAirportByIataAsync(iata1);
            var airport2 = await _airportService.GetAirportByIataAsync(iata2);

            if (airport1 == null || airport2 == null)
            {
                return NotFound("Один из аэропортов не найден.");
            }

            var distance = DistanceCalculator.CalculateDistance(
                airport1.Location.Lat, airport1.Location.Lon,
                airport2.Location.Lat, airport2.Location.Lon);

            return Ok(new
            {
                IATA1 = iata1,
                IATA2 = iata2,
                DistanceInMiles = distance
            });
        }
    }
}