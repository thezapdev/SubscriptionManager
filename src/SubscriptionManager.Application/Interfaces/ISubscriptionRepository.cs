using SubscriptionManager.Domain.Entities;

namespace SubscriptionManager.Application.Interfaces;

public interface ISubscriptionRepository
{
    Task AddSubscriptionAsync(Subscription subscription,CancellationToken cancellationToken = default);
    Task<Subscription?> GetSubscriptionByIdAsync(Guid id,CancellationToken cancellationToken = default);
    Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
