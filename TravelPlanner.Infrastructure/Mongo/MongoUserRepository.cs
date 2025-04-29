using MongoDB.Driver;
using TravelPlanner.Domain.Models;
using TravelPlanner.Domain.Interfaces;

namespace TravelPlanner.Infrastructure.Mongo
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public MongoUserRepository(IMongoDatabase mongoDatabase)
        {
            _usersCollection = mongoDatabase.GetCollection<User>("users");
        }

        public async Task CreateAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _usersCollection.Find(u => u.Email == email.ToLower()).FirstOrDefaultAsync();
        }
    }
}
