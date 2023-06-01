namespace CRMCQRS.Identity.BuilderExtensions.Options;

public class MassTransitOption
{
    public string Url { get; set; } = null!;
    public string Host { get; set; } = null!;
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
}