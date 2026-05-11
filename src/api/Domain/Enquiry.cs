namespace LaunchCart.Api.Domain;

public sealed class Enquiry
{
    public int    Id          { get; set; }
    public int    ProductId   { get; set; }
    public string Name        { get; set; } = string.Empty;
    public string Email       { get; set; } = string.Empty;
    public string Message     { get; set; } = string.Empty;
    public EnquiryStatus Status { get; set; } = EnquiryStatus.New;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public Product Product { get; set; } = null!;
}

public enum EnquiryStatus { New, Read, Replied, Closed }
