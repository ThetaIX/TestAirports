using System.Net.Http;
using System.Threading.Tasks;
using Airports.Interfaces;
using Airports.Models;
using Newtonsoft.Json;

namespace Airports.Services
{
    public class AirportService : IAirportService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AirportService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public async Task<Airport> GetAirportByIataAsync(string iataCode)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/airports/{iataCode}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Airport>(content);
        }

        public async Task<Airport?> GetAirportCoordinatesAsync(string iataCode)
        {
            if (string.IsNullOrWhiteSpace(iataCode))
            {
                return null;
            }

            string requestUrl = $"{_baseUrl}/airports/{iataCode}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API request error: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var airportData = JsonConvert.DeserializeObject<Airport>(json);

                return airportData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when receiving airport coordinates: {ex.Message}");
                return null;
            }
        }
    }
}