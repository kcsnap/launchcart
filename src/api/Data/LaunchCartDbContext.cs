using LaunchCart.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace LaunchCart.Api.Data;

public sealed class LaunchCartDbContext(DbContextOptions<LaunchCartDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products  => Set<Product>();
    public DbSet<Enquiry> Enquiries => Set<Enquiry>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Slug).IsUnique();
            e.Property(p => p.Price).HasPrecision(18, 2);
        });

        model.Entity<Enquiry>(e =>
        {
            e.HasKey(q => q.Id);
            e.HasOne(q => q.Product)
             .WithMany(p => p.Enquiries)
             .HasForeignKey(q => q.ProductId);
        });

        model.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Leather Tote Bag",    Slug = "leather-tote-bag",    Description = "Handcrafted full-grain leather tote, ideal for everyday use.",    ImageUrl = "/images/leather-tote.jpg",    Price = 149.99m },
            new Product { Id = 2, Name = "Canvas Backpack",      Slug = "canvas-backpack",      Description = "Durable waxed-canvas backpack with laptop compartment.",          ImageUrl = "/images/canvas-backpack.jpg", Price = 89.99m  },
            new Product { Id = 3, Name = "Wool Throw Blanket",   Slug = "wool-throw-blanket",   Description = "Soft merino wool throw in a classic herringbone weave.",          ImageUrl = "/images/wool-throw.jpg",      Price = 119.99m },
            new Product { Id = 4, Name = "Ceramic Coffee Mug",   Slug = "ceramic-coffee-mug",   Description = "Hand-thrown stoneware mug, microwave and dishwasher safe.",      ImageUrl = "/images/ceramic-mug.jpg",     Price = 34.99m  },
            new Product { Id = 5, Name = "Beeswax Candle Set",   Slug = "beeswax-candle-set",   Description = "Set of three hand-poured beeswax candles with cotton wicks.",    ImageUrl = "/images/beeswax-candles.jpg", Price = 44.99m  }
        );
    }
}
