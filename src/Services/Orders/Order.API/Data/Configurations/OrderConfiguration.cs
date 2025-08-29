using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.API.Models;

namespace Order.API.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Models.Order>
    {
        public void Configure(EntityTypeBuilder<Models.Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            // Id is assigned in code (Guid.NewGuid()), so prevent store generation
            builder.Property(o => o.Id).ValueGeneratedNever();

            builder.Property(o => o.UserId).IsRequired();

            builder.Property(o => o.TotalPrice)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(o => o.OrderStatus)
                   .HasConversion<int>() 
                   .IsRequired();

            builder.Property(o => o.CreatedAt)
                   .IsRequired();

            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(o => o.UserId);
        }
    }
}