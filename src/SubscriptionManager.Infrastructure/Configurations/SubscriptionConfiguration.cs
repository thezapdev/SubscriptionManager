using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionManager.Domain.Entities;
using SubscriptionManager.Domain.Enums;

namespace SubscriptionManager.Infrastructure.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions", table =>
        {
            table.HasCheckConstraint("CK_Subscriptions_Price", "\"Price\" >= 0");
            table.HasCheckConstraint("CK_Subscriptions_Name", "LENGTH(TRIM(\"Name\")) > 0");
        });
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Price).IsRequired().HasPrecision(18, 2);
        builder.Property(s => s.BillingCycle).HasConversion<string>().HasMaxLength(20).IsUnicode(false);
        builder.Property(s => s.Currency).IsRequired().HasMaxLength(3).IsFixedLength().IsUnicode(false);
        builder.Property(s => s.NextPaymentDate).IsRequired();

        builder.HasIndex(s => new { s.Status, s.NextPaymentDate });

    }
}