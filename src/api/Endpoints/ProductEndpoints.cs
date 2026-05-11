using LaunchCart.Api.Data;
using LaunchCart.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace LaunchCart.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/", async (LaunchCartDbContext db, CancellationToken ct) =>
        {
            var products = await db.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .Select(p => new ProductSummaryDto(p.Id, p.Name, p.Slug, p.ImageUrl, p.Price))
                .ToListAsync(ct);
            return Results.Ok(products);
        });

        group.MapGet("/{slug}", async (string slug, LaunchCartDbContext db, CancellationToken ct) =>
        {
            var product = await db.Products
                .Where(p => p.Slug == slug && p.IsActive)
                .Select(p => new ProductDetailDto(p.Id, p.Name, p.Slug, p.Description, p.ImageUrl, p.Price))
                .FirstOrDefaultAsync(ct);
            return product is null ? Results.NotFound() : Results.Ok(product);
        });

        // Admin endpoints
        var admin = app.MapGroup("/api/admin/products").WithTags("Admin");

        admin.MapGet("/", async (LaunchCartDbContext db, CancellationToken ct) =>
            Results.Ok(await db.Products.OrderByDescending(p => p.CreatedAtUtc).ToListAsync(ct)));

        admin.MapPost("/", async (CreateProductDto dto, LaunchCartDbContext db, CancellationToken ct) =>
        {
            var product = new Product
            {
                Name        = dto.Name,
                Slug        = dto.Slug,
                Description = dto.Description,
                ImageUrl    = dto.ImageUrl,
                Price       = dto.Price
            };
            db.Products.Add(product);
            await db.SaveChangesAsync(ct);
            return Results.Created($"/api/products/{product.Slug}", product);
        });

        admin.MapPatch("/{id:int}/deactivate", async (int id, LaunchCartDbContext db, CancellationToken ct) =>
        {
            var product = await db.Products.FindAsync([id], ct);
            if (product is null) return Results.NotFound();
            product.IsActive = false;
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        });

        return app;
    }

    public record ProductSummaryDto(int Id, string Name, string Slug, string ImageUrl, decimal Price);
    public record ProductDetailDto(int Id, string Name, string Slug, string Description, string ImageUrl, decimal Price);
    public record CreateProductDto(string Name, string Slug, string Description, string ImageUrl, decimal Price);
}
