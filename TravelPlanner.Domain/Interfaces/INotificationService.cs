namespace TravelPlanner.Domain.Interfaces
{
    public interface INotificationService
    {
        Task PublishOfferCreatedAsync(string offerId, string from, string to);
    }
}
