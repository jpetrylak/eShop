using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace eShop.Shared.Emailing;

public class SmtpEmailSender : IEmailSender
{
    private readonly IOptions<SmtpOptions> _smtpOptions;

    public SmtpEmailSender(IOptions<SmtpOptions> smtpOptions)
    {
        _smtpOptions = smtpOptions;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        MailMessage message = BuildMessage(to, subject, body);

        using var smtpClient = BuildClient();
        await smtpClient.SendMailAsync(message);
    }

    private MailMessage BuildMessage(string to, string subject, string body) =>
        new(_smtpOptions.Value.From, to, subject, body);

    private SmtpClient BuildClient()
    {
        var host = _smtpOptions.Value.Host;
        var port = _smtpOptions.Value.Port;

        var smtpClient = new SmtpClient(host, port);
        try
        {
            return smtpClient;
        }
        catch
        {
            smtpClient.Dispose();
            throw;
        }
    }
}