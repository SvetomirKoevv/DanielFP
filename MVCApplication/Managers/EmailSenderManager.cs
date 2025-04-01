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
            try
            {
                string apiKeyD = _config["EmailService:ApiKey"];
                string key = _config["EmailService:Key"];

                var apiKey = Decrypt(_config["EmailService:ApiKey"], _config["EmailService:Key"]);

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

        private static string Decrypt(string encryptedText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32)); // Ensure key is 32 bytes
            aes.IV = new byte[16]; // Default IV for simplicity

            using var decryptor = aes.CreateDecryptor();
            byte[] input = Convert.FromBase64String(encryptedText);
            byte[] decrypted = decryptor.TransformFinalBlock(input, 0, input.Length);

            return Encoding.UTF8.GetString(decrypted);
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}