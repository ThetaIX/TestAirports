using System.Net.Http;
using System.Threading.Tasks;
using Airports.Models;
using Newtonsoft.Json;

namespace Airports.Services
{
    public class AirportService
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
    }
}