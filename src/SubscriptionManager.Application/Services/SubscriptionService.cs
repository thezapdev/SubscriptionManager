using SubscriptionManager.Application.DTOs;
using SubscriptionManager.Application.Interfaces;
using SubscriptionManager.Domain.Entities;

namespace SubscriptionManager.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionrepository;
    public SubscriptionService(ISubscriptionRepository subscriptionrepository)
    {
        _subscriptionrepository = subscriptionrepository;
    }
    public async Task<SubscriptionDto> AddSubscriptionAsync(CreateSubscriptionDto subscription, CancellationToken cancellationToken = default)
    {
        var entity = new Subscription(subscription.Name,subscription.Price,subscription.BillingCycle,subscription.StartDate,subscription.Currency);
        await _subscriptionrepository.AddSubscriptionAsync(entity);
        await _subscriptionrepository.SaveChangesAsync(cancellationToken);

        return new SubscriptionDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            BillingCycle = entity.BillingCycle,
            NextPaymentDate = entity.NextPaymentDate,
            Status = entity.Status,
            Currency = entity.Currency
        };
    }

    public async Task<SubscriptionDto?> GetSubscriptionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
       var subscription = await _subscriptionrepository.GetSubscriptionByIdAsync(id);
        if (subscription is null)
        {
            return null;
        }
        
        return MapToDto(subscription);
    }

    public async Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
       var subscriptions  = await _subscriptionrepository.GetAllSubscriptionsAsync();
       return subscriptions.Select(MapToDto).ToList();
    }

    private static SubscriptionDto MapToDto(Subscription subscription)
    {
        if(subscription is null) return null;
        return new SubscriptionDto()
        {
            Id = subscription.Id,
            Name = subscription.Name,
            Price = subscription.Price,
            Currency = subscription.Currency,
            BillingCycle = subscription.BillingCycle,
            NextPaymentDate = subscription.NextPaymentDate,
            Status = subscription.Status
        };
    }
}