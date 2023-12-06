using HandlebarsDotNet;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TeduBlog.Core.ConfigOptions;
using TeduBlog.WebApp.Models;

namespace TeduBlog.WebApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSetting)
        {
            _emailSettings = emailSetting.Value;
        }

        public async Task SendEmail(EmailData emailData)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailData.FromName ?? _emailSettings.SenderName, emailData.FromEmail ?? _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(emailData.ToName ?? string.Empty, emailData.ToEmail));
            message.Subject = emailData.Subject;

            // Create the multipart/alternative MIME message
            var bodyBuilder = new BodyBuilder();

            if (!string.IsNullOrEmpty(emailData.Template))
            {
                // Read the email template
                var templatePath = emailData.Template;
                var template = File.ReadAllText(templatePath);

                // Compile the template using Handlebars
                var compiledTemplate = Handlebars.Compile(template);

                // Create the HTML body by rendering the template with dynamic data
                var htmlBody = compiledTemplate(emailData.TemplateData);

                bodyBuilder.HtmlBody = htmlBody;
            }

            // Create the text body (optional)
            var textBody = emailData.Content;

            bodyBuilder.TextBody = textBody;

            message.Body = bodyBuilder.ToMessageBody();

            // Connect to the SMTP server and send the email
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort ?? 80, false);
                await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
