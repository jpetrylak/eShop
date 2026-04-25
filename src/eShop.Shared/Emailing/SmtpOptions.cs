namespace eShop.Shared.Emailing;

public record SmtpOptions
{
    public const string Smtp = "Smtp";

    public string Host { get; init; }
    public int Port { get; init; }
    public string From { get; init; }
}