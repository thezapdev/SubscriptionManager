using SubscriptionManager.Domain.Entities;
using SubscriptionManager.Domain.Enums;
using SubscriptionManager.Domain.Exceptions;

namespace SubscriptionManager.UnitTests;

public class SubscriptionTests
{
    private static Subscription CreateValid() =>
        new("Netflix", 49.99m, BillingCycle.Monthly, new DateOnly(2026, 12, 1));

    [Fact]
    public void Constructor_WithValidData_CreatesActiveSubscription()
    {
        var sub = CreateValid();

        Assert.Equal(SubscriptionStatus.Active, sub.Status);
        Assert.Equal("Netflix", sub.Name);
        Assert.NotEqual(Guid.Empty, sub.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyName_Throws(string name)
    {
        Assert.Throws<DomainException>(() =>
            new Subscription(name, 10m, BillingCycle.Monthly, new DateOnly(2026, 12, 1)));
    }

    [Fact]
    public void Constructor_WithNegativePrice_Throws()
    {
        Assert.Throws<DomainException>(() =>
            new Subscription("Spotify", -1m, BillingCycle.Monthly, new DateOnly(2026, 12, 1)));
    }

    [Fact]
    public void Cancel_ActiveSubscription_SetsStatusToCancelled()
    {
        var sub = CreateValid();

        sub.Cancel();

        Assert.Equal(SubscriptionStatus.Cancelled, sub.Status);
    }

    [Fact]
    public void Cancel_AlreadyCancelled_Throws()
    {
        var sub = CreateValid();
        sub.Cancel();

        Assert.Throws<InvalidSubscriptionStateException>(() => sub.Cancel());
    }

    [Fact]
    public void Resume_CancelledSubscription_SetsActiveAndNewDate()
    {
        var sub = CreateValid();
        sub.Cancel();
        var newDate = new DateOnly(2027, 1, 15);

        sub.Resume(newDate);

        Assert.Equal(SubscriptionStatus.Active, sub.Status);
        Assert.Equal(newDate, sub.NextPaymentDate);
    }

    [Fact]
    public void Resume_ActiveSubscription_Throws()
    {
        var sub = CreateValid();

        Assert.Throws<InvalidSubscriptionStateException>(() => sub.Resume(new DateOnly(2027, 1, 1)));
    }
}