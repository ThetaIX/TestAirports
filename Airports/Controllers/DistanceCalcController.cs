using Airports.Interfaces;
using Airports.Models;
using Airports.Services;
using Airports.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Airports.Controllers
{
    [ApiController]
    [Route("api/distance")]
    public class DistanceCalcController : ControllerBase
    {
        private readonly IDistanceService _distanceService;

        public DistanceCalcController(IDistanceService distanceService)
        {
            _distanceService = distanceService;
        }

        [HttpGet]
        public async Task<IActionResult> CalculateDistance(string iata1, string iata2)
        {
            DistRequest request = new DistRequest() { FromIata = iata1, ToIata = iata2 };
            if (request == null || string.IsNullOrWhiteSpace(request.FromIata) || string.IsNullOrWhiteSpace(request.ToIata))
            {
                return BadRequest(new { message = "Airport codes cannot be blank." });
            }

            var response = await _distanceService.CalculateDistanceAsync(request);

            if (response == null)
            {
                return NotFound(new { message = "One or both airports are not found." });
            }

            return Ok(response);
        }
    }
}