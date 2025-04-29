using System.Threading.Tasks;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Interfaces
{
    public interface IOfferCreationService
    {
        Task CreateOfferAsync(Offer offer);
    }
}
