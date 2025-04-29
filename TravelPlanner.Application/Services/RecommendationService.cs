using TravelPlanner.Domain.Interfaces;

namespace TravelPlanner.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IRecommendationRepository _recommendationRepository;
        public RecommendationService(IRecommendationRepository recommendationRepository)
        {
            _recommendationRepository = recommendationRepository;
        }

        public async Task<IEnumerable<string>> GetRecommendationsAsync(string city, int k)
        {
            return await _recommendationRepository.GetRecommendationsAsync(city, k);
        }
    }
}
