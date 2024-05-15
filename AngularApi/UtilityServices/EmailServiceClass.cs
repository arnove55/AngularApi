using AngularApi.Models;
using MailKit.Net.Smtp;
using MimeKit;


namespace AngularApi.UtilityServices
{
    public class EmailServiceClass : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailServiceClass(IConfiguration config)
        {
            _config = config;
        }
        public void EndEmail(Emailmodel emailmodel)
        {
            var emailmsg = new MimeMessage();
            var from = _config["EmailSettings:From"];

            emailmsg.From.Add(new MailboxAddress("MealBook", from));
            emailmsg.To.Add(new MailboxAddress(emailmodel.To, emailmodel.To)); 
            emailmsg.Subject = emailmodel.Subject; 

            emailmsg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailmodel.Content // Use the content provided in the emailmodel
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_config["EmailSettings:SmtpServer"], 465, true); // Use secure connection
                    client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                    client.Send(emailmsg);
                }
                catch (Exception ex)
                {
                    // Log or return error message
                    throw new Exception("Failed to send email.", ex);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

    }
}
