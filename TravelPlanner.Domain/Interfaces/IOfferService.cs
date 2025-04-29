using TravelPlanner.Domain.Models;
using TravelPlanner.Domain.DTOs;
namespace TravelPlanner.Domain.Interfaces
{
    public interface IOfferService
    {
        Task<IEnumerable<Offer>> SearchOffersAsync(string from, string to, int limit);
        Task<OfferDetailsResponse?> GetOfferDetailsAsync(string id);
    }
}
