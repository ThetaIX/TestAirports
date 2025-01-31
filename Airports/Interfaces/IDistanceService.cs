using Airports.Models;

namespace Airports.Interfaces
{
    public interface IDistanceService
    {
        Task<DistResponse> CalculateDistanceAsync(DistRequest request);


    }
}
