using System.Text.Json;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Application.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly ICacheService _cacheService;

        public OfferService(IOfferRepository offerRepository, ICacheService cacheService)
        {
            _offerRepository = offerRepository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Offer>> SearchOffersAsync(string from, string to, int limit)
        {
            var cacheKey = $"offers:{from}:{to}";
            var cached = await _cacheService.GetCompressedJsonAsync(cacheKey);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<IEnumerable<Offer>>(cached) ?? Enumerable.Empty<Offer>();
            }

            var results = await _offerRepository.SearchOffersAsync(from, to, limit);
            var json = JsonSerializer.Serialize(results);

            await _cacheService.SetCompressedJsonAsync(cacheKey, json, TimeSpan.FromSeconds(60));

            return results;
        }

        public async Task<Offer?> GetOfferDetailsAsync(string id)
        {
            var cacheKey = $"offers:{id}";
            var cached = await _cacheService.GetCompressedJsonAsync(cacheKey);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<Offer>(cached);
            }

            var offer = await _offerRepository.GetOfferByIdAsync(id);

            if (offer != null)
            {
                var json = JsonSerializer.Serialize(offer);
                await _cacheService.SetCompressedJsonAsync(cacheKey, json, TimeSpan.FromSeconds(300));
            }

            return offer;
        }
    }
}
