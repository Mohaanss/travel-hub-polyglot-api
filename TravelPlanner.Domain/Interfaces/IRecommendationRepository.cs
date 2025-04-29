namespace TravelPlanner.Domain.Interfaces
{
    public interface IRecommendationRepository
    {
        Task<IEnumerable<string>> GetRecommendationsAsync(string city, int k);
    }
}
