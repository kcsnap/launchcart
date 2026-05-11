using LaunchCart.Api.Data;
using LaunchCart.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace LaunchCart.Api.Endpoints;

public static class EnquiryEndpoints
{
    public static IEndpointRouteBuilder MapEnquiryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/enquiries").WithTags("Enquiries");

        group.MapPost("/", async (SubmitEnquiryDto dto, LaunchCartDbContext db, CancellationToken ct) =>
        {
            var product = await db.Products.FindAsync([dto.ProductId], ct);
            if (product is null || !product.IsActive) return Results.BadRequest("Product not found.");

            var enquiry = new Enquiry
            {
                ProductId = dto.ProductId,
                Name      = dto.Name,
                Email     = dto.Email,
                Message   = dto.Message
            };
            db.Enquiries.Add(enquiry);
            await db.SaveChangesAsync(ct);
            return Results.Created($"/api/admin/enquiries/{enquiry.Id}", new { enquiry.Id });
        });

        // Admin endpoints
        var admin = app.MapGroup("/api/admin/enquiries").WithTags("Admin");

        admin.MapGet("/", async (LaunchCartDbContext db, CancellationToken ct) =>
            Results.Ok(await db.Enquiries
                .Include(q => q.Product)
                .OrderByDescending(q => q.CreatedAtUtc)
                .ToListAsync(ct)));

        admin.MapPatch("/{id:int}/status", async (int id, UpdateStatusDto dto, LaunchCartDbContext db, CancellationToken ct) =>
        {
            var enquiry = await db.Enquiries.FindAsync([id], ct);
            if (enquiry is null) return Results.NotFound();
            enquiry.Status = dto.Status;
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        });

        return app;
    }

    public record SubmitEnquiryDto(int ProductId, string Name, string Email, string Message);
    public record UpdateStatusDto(EnquiryStatus Status);
}
