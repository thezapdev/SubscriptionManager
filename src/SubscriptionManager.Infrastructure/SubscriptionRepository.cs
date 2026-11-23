using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Application.Interfaces;
using SubscriptionManager.Domain.Entities;

namespace SubscriptionManager.Infrastructure;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ApplicationDbContext _context;
    public SubscriptionRepository(ApplicationDbContext context)
    {
       _context = context;
    }

    public async Task AddSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        await _context.Subscriptions.AddAsync(subscription,cancellationToken);
    }

    public async Task<Subscription?> GetSubscriptionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id, cancellationToken); 
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        return await  _context.Subscriptions.AsNoTracking().ToListAsync(cancellationToken); 
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        _context.Subscriptions.Remove(subscription);
    }
}