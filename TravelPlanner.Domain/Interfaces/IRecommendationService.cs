using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlanner.Domain.Interfaces
{
    public interface IRecommendationService
    {
        Task<IEnumerable<string>> GetRecommendationsAsync(string city, int k);
    }
}
