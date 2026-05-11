using LaunchCart.Api.Domain;
using Xunit;

namespace LaunchCart.Api.Tests;

public sealed class DomainTests
{
    [Fact]
    public void Product_DefaultsToActive()
    {
        var product = new Product { Name = "Test", Slug = "test", Price = 10m };
        Assert.True(product.IsActive);
    }

    [Fact]
    public void Enquiry_DefaultsToNewStatus()
    {
        var enquiry = new Enquiry { Name = "Alice", Email = "alice@example.com", Message = "Hello" };
        Assert.Equal(EnquiryStatus.New, enquiry.Status);
    }

    [Fact]
    public void EnquiryStatus_HasExpectedValues()
    {
        Assert.Equal(4, Enum.GetValues<EnquiryStatus>().Length);
    }
}
