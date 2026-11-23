using SubscriptionManager.Application.DTOs;
using SubscriptionManager.Domain.Entities;

namespace SubscriptionManager.Application.Interfaces;

public interface ISubscriptionService
{ 
    Task<SubscriptionDto> AddSubscriptionAsync(CreateSubscriptionDto subscription,CancellationToken cancellationToken = default);
    Task<SubscriptionDto> GetSubscriptionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default);
    Task DeleteSubscriptionAsync(Guid id, CancellationToken cancellationToken = default);
    Task ResumeSubscriptionAsync(Guid id,DateOnly dateOnly, CancellationToken cancellationToken = default);
    Task CancelSubscriptionAsync(Guid id, CancellationToken cancellationToken = default);
    Task RenewSubscriptionAsync(Guid id, CancellationToken cancellationToken = default);
}