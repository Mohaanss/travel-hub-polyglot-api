using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task<User?> GetByEmailAsync(string email);
    }
}
