namespace LaunchCart.Api.Domain;

public sealed class Product
{
    public int    Id          { get; set; }
    public string Name        { get; set; } = string.Empty;
    public string Slug        { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl    { get; set; } = string.Empty;
    public decimal Price      { get; set; }
    public bool   IsActive    { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Enquiry> Enquiries { get; set; } = new List<Enquiry>();
}
