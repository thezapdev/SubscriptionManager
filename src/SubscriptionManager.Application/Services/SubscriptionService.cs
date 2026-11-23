using SubscriptionManager.Application.DTOs;
using SubscriptionManager.Application.Exceptions;
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
        var currency = Currency.FromCode(subscription.Currency);
        var entity = new Subscription(
            subscription.Name,
            subscription.Price,
            subscription.BillingCycle,
            subscription.StartDate,
            currency);
        await _subscriptionrepository.AddSubscriptionAsync(entity,cancellationToken);
        await _subscriptionrepository.SaveChangesAsync(cancellationToken);

        return new SubscriptionDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Price =  entity.Price,
            BillingCycle = entity.BillingCycle,
            NextPaymentDate = entity.NextPaymentDate,
            Status = entity.Status,
            Currency = entity.Currency.Code
        };
    }

    public async Task<SubscriptionDto?> GetSubscriptionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
       var subscription = await _subscriptionrepository.GetSubscriptionByIdAsync(id,cancellationToken);
        if (subscription is null)
        {
            throw new NotFoundException(nameof(Subscription), id);
        }
        
        return MapToDto(subscription);
    }

    public async Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
       var subscriptions  = await _subscriptionrepository.GetAllSubscriptionsAsync(cancellationToken);
       return subscriptions.Select(MapToDto).ToList();
    }

    public async Task DeleteSubscriptionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionrepository.GetSubscriptionByIdAsync(id, cancellationToken);
        if (subscription is null)
        {
            throw new NotFoundException(nameof(Subscription), id);
        }
        await _subscriptionrepository.DeleteSubscriptionAsync(subscription, cancellationToken);
        await _subscriptionrepository.SaveChangesAsync(cancellationToken);
        

    }

    public async Task ResumeSubscriptionAsync(Guid id,DateOnly dateOnly, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionrepository.GetSubscriptionByIdAsync(id, cancellationToken);
        if (subscription is null)
        {
            throw new NotFoundException(nameof(Subscription), id);
        }   
        subscription.Resume(dateOnly);
        await _subscriptionrepository.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelSubscriptionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionrepository.GetSubscriptionByIdAsync(id, cancellationToken);
        if (subscription is null)
        {
            throw new NotFoundException(nameof(Subscription), id);
        }
        subscription.Cancel();
        await _subscriptionrepository.SaveChangesAsync(cancellationToken);
    }

    public async Task RenewSubscriptionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionrepository.GetSubscriptionByIdAsync(id, cancellationToken);

        if (subscription is null)
        {
            throw new NotFoundException(nameof(Subscription), id);

        }
        subscription.Renew();
        await _subscriptionrepository.SaveChangesAsync(cancellationToken); 
    }


    private static SubscriptionDto MapToDto(Subscription subscription)
    {
        return new SubscriptionDto()
        {
            Id = subscription.Id,
            Name = subscription.Name,
            Price = subscription.Price,
            Currency = subscription.Currency.Code,
            BillingCycle = subscription.BillingCycle,
            NextPaymentDate = subscription.NextPaymentDate,
            Status = subscription.Status
        };
    }
}