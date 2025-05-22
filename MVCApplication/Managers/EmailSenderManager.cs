using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;
using System.Security.Cryptography;


namespace MVCApplication.Managers
{
    public class EmailSenderManager : IEmailSender
    {
        public static async Task<Response> SendEmailAsync(string email, string username, string subject, string body, IConfiguration _config)
        {
            throw new NotImplementedException();
        }


        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}