namespace CRMCQRS.Identity.BuilderExtensions.Options;

public class IdentityAddressOption
{
    public string Authority { get; set; } = null!;
    public string? Audience { get; set; }
}