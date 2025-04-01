using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace MVCApplication.Managers
{
    public class EmailSenderManager : IEmailSender
    {

        public static async Task<Response> SendEmailAsync(string email, string username, string subject, string body)
        {
            try
            {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress("dankata047@gmail.com", "BiteBliss Admin");

                var to = new EmailAddress("dankata47@abv.bg", "EmailReciever");

                var htmlContent = $"<div style=\"display:flex; widht: 10%;\">" +
                                    $"<h3 style=\"font-weight: 100; width: 50%; text-align: center;\">User: {username}  /  {email}</h3>" +
                                    $"<hr>" +
                                    $"<h3 style=\"font-weight: 100; width: 50%; text-align: center;\">Sibject: {subject}</h3>" +
                                    $"</div>" +
                                    $"<hr>" +
                                    $"<h3 style=\"text-align: center;\">{body}</h3>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}