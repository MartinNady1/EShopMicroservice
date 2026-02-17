using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasConversion(OrderId => OrderId.Value, dbId => OrderId.Of(dbId));
            builder.HasOne<Customer>().WithMany().HasForeignKey(o => o.CustomerId).IsRequired();
            builder.HasMany(o => o.OrderItems)  // Changed from HasMany<OrderItem>()
           .WithOne()
           .HasForeignKey(oi => oi.OrderId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.ComplexProperty(o => o.OrderName,
                nameBuilder =>
                {
                    nameBuilder.IsRequired();
                    nameBuilder.Property(n => n.Value)
                        .HasMaxLength(50)
                        .IsRequired();

                });
            builder.OwnsOne(o => o.ShippingAddress,
                addressBuilder =>
                {

                    addressBuilder.Property(a => a.FirstName)
                        .HasMaxLength(50)
                        .IsRequired();

                    addressBuilder.Property(a => a.LastName)
                        .HasMaxLength(50)
                        .IsRequired();
                    addressBuilder.Property(a => a.EmailAddress)
                        .HasMaxLength(100)
                        .IsRequired();
                    addressBuilder.Property(a => a.City)
                        .HasMaxLength(50)
                        .IsRequired();
                    addressBuilder.Property(a => a.State)
                        .HasMaxLength(50)
                        .IsRequired();
                    addressBuilder.Property(a => a.ZipCode)
                        .HasMaxLength(50)
                        .IsRequired();
                    addressBuilder.Property(a => a.AddressLine)
                        .HasMaxLength(100)
                        .IsRequired();

                });
            builder.OwnsOne(o => o.BillingAddress,
                addressBuilder =>
                {
                    addressBuilder.Property(a => a.FirstName)
                        .HasMaxLength(50)
                        .IsRequired();
                    addressBuilder.Property(a => a.LastName)
                        .HasMaxLength(50)
                        .IsRequired();
                    addressBuilder.Property(a => a.EmailAddress)
                        .HasMaxLength(100);
                    addressBuilder.Property(a => a.City)
                        .HasMaxLength(50)
                        .IsRequired();
                    addressBuilder.Property(a => a.State)
                        .HasMaxLength(50)
                        .IsRequired();
                    addressBuilder.Property(a => a.ZipCode)
                        .HasMaxLength(50)
                        .IsRequired();
                    addressBuilder.Property(a => a.AddressLine)
                        .HasMaxLength(100)
                        .IsRequired();
                    addressBuilder.Property(a => a.Country)
                        .HasMaxLength(50)
                        .IsRequired();
                });
            builder.OwnsOne(o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(p => p.CardHolderName)
                    .HasMaxLength(50)
                    .IsRequired();
                paymentBuilder.Property(p => p.CardNumber)
                    .HasMaxLength(24)
                    .IsRequired();
                paymentBuilder.Property(p => p.ExpirationDate)
                    .HasMaxLength(10)
                    .IsRequired();
                paymentBuilder.Property(p => p.CVV)
                    .HasMaxLength(3)
                    .IsRequired();
                paymentBuilder.Property(p => p.PaymentMethod)
                    .IsRequired();
            });

        }
    }
}