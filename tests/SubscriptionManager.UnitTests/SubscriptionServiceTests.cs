
using FluentValidation;
using NSubstitute;
using Xunit;
using SubscriptionManager.Application.Interfaces;
using SubscriptionManager.Application.Services;
using SubscriptionManager.Application.Exceptions;
using SubscriptionManager.Domain.Entities;
using SubscriptionManager.Domain.Enums;
using SubscriptionManager.Domain.Exceptions;
using SubscriptionManager.Application.DTOs;
using SubscriptionManager.Application.Validators;

namespace SubscriptionManager.UnitTests;

public class SubscriptionServiceTests
{
    private readonly ISubscriptionRepository _repoMock;
    private readonly SubscriptionService _sut;
    private readonly IValidator<CreateSubscriptionDto> _validator;

    public SubscriptionServiceTests()
    {
        _repoMock = Substitute.For<ISubscriptionRepository>();
        _validator = new CreateSubscriptionDtoValidator();
        _sut = new SubscriptionService(_repoMock,_validator);
    }

    [Fact]
    public async Task CancelSubscription_WithValidId_ChangesStatusToCancelled()
    {
        // Arrange
        var sub = CreateValidSubscription();

        _repoMock.GetSubscriptionByIdAsync(sub.Id).Returns(sub);

        // Act
        await _sut.CancelSubscriptionAsync(sub.Id);

        // Assert
        Assert.Equal(SubscriptionStatus.Cancelled, sub.Status);
        await _repoMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task CancelSubscription_WithInvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock.GetSubscriptionByIdAsync(id)
            .Returns((Subscription)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _sut.CancelSubscriptionAsync(id));
    }

    [Fact]
    public async Task RenewSubscription_WithValidId_RenewsSubscription()
    {
        // Arrange
        var sub = CreateValidSubscription(
            cycle: BillingCycle.Monthly);

        var originalDate = sub.NextPaymentDate;

        _repoMock.GetSubscriptionByIdAsync(sub.Id).Returns(sub);

        // Act
        await _sut.RenewSubscriptionAsync(sub.Id);

        // Assert
        Assert.True(sub.NextPaymentDate > originalDate);
        await _repoMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task RenewSubscription_WithInvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock.GetSubscriptionByIdAsync(id)
            .Returns((Subscription)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _sut.RenewSubscriptionAsync(id));
    }

    [Fact]
    public async Task GetSubscriptionById_WithValidId_ReturnsSubscription()
    {
        // Arrange
        var sub = CreateValidSubscription();

        _repoMock.GetSubscriptionByIdAsync(sub.Id).Returns(sub);

        // Act
        var result = await _sut.GetSubscriptionByIdAsync(sub.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sub.Id, result.Id);
        Assert.Equal(sub.Name, result.Name);
    }

    [Fact]
    public async Task GetAllSubscriptions_ReturnsAllSubscriptions()
    {
        // Arrange
        var subscriptions = new List<Subscription>
        {
            CreateValidSubscription(),
            CreateValidSubscription("Spotify", 15m)
        };

        _repoMock.GetAllSubscriptionsAsync()
            .Returns(subscriptions);

        // Act
        var result = await _sut.GetAllSubscriptionsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task DeleteSubscription_WithValidId_DeletesSubscription()
    {
        // Arrange
        var sub = CreateValidSubscription();

        _repoMock.GetSubscriptionByIdAsync(sub.Id).Returns(sub);

        // Act
        await _sut.DeleteSubscriptionAsync(sub.Id);

        // Assert
        await _repoMock.Received(1).DeleteSubscriptionAsync(sub);
        await _repoMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task ResumeSubscription_WithValidId_ChangesStatusToActive()
    {
        // Arrange
        var sub = CreateValidSubscription();
        sub.Cancel();

        var resumeDate = DateOnly.FromDateTime(
            DateTime.Today.AddDays(7));

        _repoMock.GetSubscriptionByIdAsync(sub.Id).Returns(sub);

        // Act
        await _sut.ResumeSubscriptionAsync(sub.Id, resumeDate);

        // Assert
        Assert.Equal(SubscriptionStatus.Active, sub.Status);
        await _repoMock.Received(1).SaveChangesAsync();
    }

    private static Subscription CreateValidSubscription(
        string name = "Netflix",
        decimal price = 10m,
        BillingCycle cycle = BillingCycle.Monthly)
    {
        return new Subscription(
            name,
            price,
            cycle,
            DateOnly.FromDateTime(DateTime.Today),
            Currency.EUR);
    }
}
