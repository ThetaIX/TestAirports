using Airports.Models;

namespace Airports.Interfaces
{
    public interface IDistanceCacheService
    {
        Task<DistResponse?> GetCachedDistanceAsync(string airportCode1, string airportCode2);
        Task SaveDistanceAsync(DistResponse response);
    }

}
