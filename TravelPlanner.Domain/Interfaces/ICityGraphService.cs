using System.Threading.Tasks;

namespace TravelPlanner.Domain.Interfaces
{
    public interface ICityGraphService
    {
        Task EnsureCitiesAndRelationAsync(string fromCityCode, string toCityCode, double weight);
    }
}
