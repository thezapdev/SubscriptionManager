using FluentValidation.TestHelper;
using Xunit;
using SubscriptionManager.Application.DTOs;
using SubscriptionManager.Application.Validators;
using SubscriptionManager.Domain.Enums;

namespace SubscriptionManager.UnitTests;

public class CreateSubscriptionDtoValidatorTests
{
    private readonly CreateSubscriptionDtoValidator _sut;

    public CreateSubscriptionDtoValidatorTests()
    {
        _sut = new CreateSubscriptionDtoValidator();
    }

    [Fact]
    public void Validate_WithValidDto_HasNoValidationErrors()
    {
        // Arrange
        var dto = CreateValidDto();

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    // ---------- Name ----------

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_WithEmptyName_HasValidationError(string? name)
    {
        // Arrange
        var dto = CreateValidDto(name: name!);

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name is required");
    }

    [Fact]
    public void Validate_WithNameLongerThan30Characters_HasValidationError()
    {
        // Arrange
        var dto = CreateValidDto(name: new string('a', 31));

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name cannot exceed 30 characters");
    }

    [Fact]
    public void Validate_WithNameOfExactly30Characters_HasNoValidationError()
    {
        // Arrange
        var dto = CreateValidDto(name: new string('a', 30));

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    // ---------- Price ----------

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-49.99)]
    public void Validate_WithZeroOrNegativePrice_HasValidationError(double price)
    {
        // Arrange
        var dto = CreateValidDto(price: (decimal)price);

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price)
            .WithErrorMessage("Price must be greater than 0");
    }

    [Fact]
    public void Validate_WithPositivePrice_HasNoValidationError()
    {
        // Arrange
        var dto = CreateValidDto(price: 0.01m);

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
    }

    // ---------- Currency ----------

    [Theory]
    [InlineData("PLN")]
    [InlineData("EUR")]
    [InlineData("USD")]
    public void Validate_WithAllowedCurrency_HasNoValidationError(string currency)
    {
        // Arrange
        var dto = CreateValidDto(currency: currency);

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Currency);
    }

    [Theory]
    [InlineData("GBP")]
    [InlineData("pln")]
    [InlineData("PLNN")]
    [InlineData("")]
    public void Validate_WithNotAllowedCurrency_HasValidationError(string currency)
    {
        // Arrange
        var dto = CreateValidDto(currency: currency);

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Currency)
            .WithErrorMessage("Allowed currencies are: PLN, EUR, USD");
    }

    // ---------- BillingCycle ----------

    [Theory]
    [InlineData(BillingCycle.Weekly)]
    [InlineData(BillingCycle.Monthly)]
    [InlineData(BillingCycle.Quarterly)]
    [InlineData(BillingCycle.SemiAnually)]
    [InlineData(BillingCycle.Yearly)]
    [InlineData(BillingCycle.Biennial)]
    [InlineData(BillingCycle.Custom)]
    public void Validate_WithValidBillingCycle_HasNoValidationError(BillingCycle cycle)
    {
        // Arrange
        var dto = CreateValidDto(billingCycle: cycle);

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.BillingCycle);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(8)]
    [InlineData(-1)]
    [InlineData(999)]
    public void Validate_WithInvalidBillingCycle_HasValidationError(int value)
    {
        // Arrange
        var dto = CreateValidDto(billingCycle: (BillingCycle)value);

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BillingCycle)
            .WithErrorMessage("The billing cycle is not valid");
    }

    // ---------- StartDate ----------

    [Fact]
    public void Validate_WithStartDateOlderThan30Days_HasValidationError()
    {
        // Arrange
        var dto = CreateValidDto(
            startDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-31)));

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StartDate)
            .WithErrorMessage("Start date cannot be older than 30 days");
    }

    [Theory]
    [InlineData(-29)]
    [InlineData(0)]
    [InlineData(30)]
    public void Validate_WithStartDateWithinAllowedRange_HasNoValidationError(int daysOffset)
    {
        // Arrange
        var dto = CreateValidDto(
            startDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(daysOffset)));

        // Act
        var result = _sut.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.StartDate);
    }

    private static CreateSubscriptionDto CreateValidDto(
        string name = "Netflix",
        decimal price = 49.99m,
        string currency = "PLN",
        BillingCycle billingCycle = BillingCycle.Monthly,
        DateOnly? startDate = null)
    {
        return new CreateSubscriptionDto
        {
            Name = name,
            Price = price,
            Currency = currency,
            BillingCycle = billingCycle,
            StartDate = startDate ?? DateOnly.FromDateTime(DateTime.UtcNow)
        };
    }
}