using System.Text.Json;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Domain.Models;
using TravelPlanner.Domain.DTOs;
namespace TravelPlanner.Application.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly ICacheService _cacheService;
        private readonly ICityGraphRepository _cityGraphRepository;


        public OfferService(IOfferRepository offerRepository, ICacheService cacheService, ICityGraphRepository cityGraphRepository
        )
        {
            _offerRepository = offerRepository;
            _cacheService = cacheService;
            _cityGraphRepository = cityGraphRepository;
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

        public async Task<OfferDetailsResponse?> GetOfferDetailsAsync(string id)
        {
            var cacheKey = $"offers:{id}";
            var cached = await _cacheService.GetCompressedJsonAsync(cacheKey);

            if (cached != null)
                return JsonSerializer.Deserialize<OfferDetailsResponse>(cached);

            var offer = await _offerRepository.GetOfferByIdAsync(id);
            if (offer == null) return null;

            var related = await _cityGraphRepository.GetRelatedOffersAsync(
                offer.From, offer.DepartDate, offer.Id);

            var response = new OfferDetailsResponse
            {
                Offer = offer,
                RelatedOffers = related
            };

            var json = JsonSerializer.Serialize(response);
            await _cacheService.SetCompressedJsonAsync(cacheKey, json, TimeSpan.FromSeconds(300));

            return response;
        }
    }
}
