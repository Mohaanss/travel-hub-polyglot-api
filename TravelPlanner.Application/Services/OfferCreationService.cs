using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Domain.Models;
using TravelPlanner.Application.Helpers;
namespace TravelPlanner.Application.Services
{
    public class OfferCreationService : IOfferCreationService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly ICityGraphService _cityGraphService;
        private readonly INotificationService _notificationService;
        public OfferCreationService(IOfferRepository offerRepository, ICityGraphService cityGraphService, INotificationService notificationService)
        {
            _offerRepository = offerRepository;
            _notificationService = notificationService;
            _cityGraphService = cityGraphService;
        }

        public async Task CreateOfferAsync(Offer offer)
        {
            var fromCoord = CityCoordinates.Get(offer.From);
            var toCoord = CityCoordinates.Get(offer.To);
            if (fromCoord == null || toCoord == null)
                throw new Exception("Coordonnées inconnues pour une des villes");
            var distance = CityDistanceService.ComputeDistance(fromCoord.Value.Lat, fromCoord.Value.Lon, toCoord.Value.Lat, toCoord.Value.Lon);
            var weight = CityDistanceService.ComputeWeight(distance);
            // 1. Créer l'offre dans MongoDB
            await _offerRepository.CreateOfferAsync(offer);
            await _notificationService.PublishOfferCreatedAsync(offer.Id, offer.From, offer.To);

            // 2. Créer les villes + la relation dans Neo4j
            await _cityGraphService.EnsureCitiesAndRelationAsync(offer.From, offer.To, weight);
        }
    }
}
