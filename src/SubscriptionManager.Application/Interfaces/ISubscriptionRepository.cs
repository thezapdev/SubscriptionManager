using SubscriptionManager.Domain.Entities;

namespace SubscriptionManager.Application.Interfaces;

public interface ISubscriptionRepository
{
    Task AddSubscriptionAsync(Subscription subscription);
    Task<Subscription?> GetSubscriptionByIdAsync(Guid id);
    Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
