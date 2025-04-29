using MongoDB.Driver;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Infrastructure.Mongo
{
    public class MongoOfferRepository : IOfferRepository
    {
        private readonly IMongoCollection<Offer> _offers;

        public MongoOfferRepository(IMongoDatabase database)
        {
            _offers = database.GetCollection<Offer>("offers");
        }

        public async Task<List<Offer>> SearchOffersAsync(string from, string to, int limit)
        {
            var filter = Builders<Offer>.Filter.Eq(o => o.From, from) &
                         Builders<Offer>.Filter.Eq(o => o.To, to);

            return await _offers
                .Find(filter)
                .SortBy(o => o.Price)
                .Limit(limit)
                .ToListAsync();
        }

        public async Task<Offer?> GetOfferByIdAsync(string id)
        {
            var filter = Builders<Offer>.Filter.Eq(o => o.Id, id);
            Console.WriteLine(filter);
            return await _offers.Find(filter).FirstOrDefaultAsync();
        }
        public async Task CreateOfferAsync(Offer offer)
        {
            await _offers.InsertOneAsync(offer);
        }
    }
}
