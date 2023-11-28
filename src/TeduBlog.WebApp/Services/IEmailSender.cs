using TeduBlog.WebApp.Models;

namespace TeduBlog.WebApp.Services
{
    public interface IEmailSender
    {
        Task SendEmail(EmailData emailData);
    }
}
