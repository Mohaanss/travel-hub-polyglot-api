using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Interfaces
{
    public interface IOfferRepository
    {
        Task<List<Offer>> SearchOffersAsync(string from, string to, int limit);
        Task<Offer?> GetOfferByIdAsync(string id);
        Task CreateOfferAsync(Offer offer);

    }
}
