using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Interfaces
{
    public interface IOfferService
    {
        Task<IEnumerable<Offer>> SearchOffersAsync(string from, string to, int limit);
        Task<Offer?> GetOfferDetailsAsync(string id);
    }
}
