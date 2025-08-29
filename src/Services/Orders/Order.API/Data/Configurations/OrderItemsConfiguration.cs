using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.API.Models;

namespace Order.API.Data.Configurations
{
    public class OrderItemsConfiguration : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Id).ValueGeneratedNever();

            builder.Property(oi => oi.OrderId).IsRequired();

            builder.Property(oi => oi.CourseId).IsRequired();

            builder.Property(oi => oi.Price)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(oi => new { oi.OrderId, oi.CourseId });
        }
    }
}