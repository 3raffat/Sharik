using Microsoft.Extensions.Configuration;
using MimeKit;
using Sharik.Domain.Auth;
using Sharik.Domain.Common.Results;

namespace Sharik.Infrastructure.Auth
{
    public sealed class EmailService(IConfiguration _cfg) : IEmailService
    {

        public async Task<Result<Success>> SendConfirmationEmailAsync(string to , string userName , string confirmationLink , CancellationToken ct)
        {

            var htmlTemplate = await LoadTemplateAsync("ConfirmEmailTemplate.html");
            string htmlBody = htmlTemplate.Value.Replace("{{UserName}}" , userName)
                                                .Replace("{{ConfirmationLink}}" , confirmationLink);



           var sendResult = await SendEmailAsync(to , "Confirm Your Email" , htmlBody , ct);

            if (sendResult.IsFailure)
                return sendResult.Errors;



            return Result.Success;
        }

        private async Task<Result<Success>> SendEmailAsync(string to , string subject ,string htmlBody  , CancellationToken ct)
        {
            var emaillSettings = _cfg.GetSection("EmailSettings");
            var email = new MimeKit.MimeMessage();


            var from = new MailboxAddress(emaillSettings["Username"] , emaillSettings["From"]!);
            var toAddress = MailboxAddress.Parse(to);
            email.From.Add(from);
            email.To.Add(toAddress);
            email.Subject = $"{subject} :{to}";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlBody
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            var port = int.Parse(emaillSettings["Port"]!);

            await smtp.ConnectAsync(emaillSettings["SmtpHost"] , port , MailKit.Security.SecureSocketOptions.StartTls , ct);
            await smtp.AuthenticateAsync(emaillSettings["From"] , emaillSettings["Password"] , ct);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true , ct);

            return Result.Success;
        }

        private async Task<Result<string>> LoadTemplateAsync(string template)
        {
            var assembly = typeof(EmailService).Assembly;

            var resourceName = assembly
                  .GetManifestResourceNames()
                  .FirstOrDefault(x => x.EndsWith(template , StringComparison.OrdinalIgnoreCase));
 

            if (resourceName == null)
                return AuthErrors.TemplateNotFound;

            await using var stream = assembly.GetManifestResourceStream(resourceName);


            if (stream is null)
                return AuthErrors.TemplateNotFound;

            using var reader = new StreamReader(stream!);

            return await reader.ReadToEndAsync();
        }

    }
}

public interface IEmailService
{
    Task<Result<Success>> SendConfirmationEmailAsync(string to , string userName , string confirmationLink , CancellationToken ct);

}
