using Basket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configuration;
public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.ProductId).IsRequired();
        builder.Property(s => s.Quantity).IsRequired();
        builder.Property(s => s.Price).IsRequired();
        builder.Property(s => s.ProductName).IsRequired();
    }
}
