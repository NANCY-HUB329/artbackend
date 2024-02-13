using EmailService.Models;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Threading.Tasks;
using OrderService.Models;

namespace EmailService.Services
{
	public class MailService
	{
		private readonly IConfiguration _configuration;

		public MailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

        internal async Task SendOrderConfirmation(OrderAzure? orderAzure)
        {
			string useremail = orderAzure.email;
			string productName = orderAzure?.artName;
            double amount = orderAzure?.amount?? 0.0;
            string username = "user"; // Consider using the actual username if available

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("TheeArtSpace", "mwanginancy333@gmail.com"));
                message.To.Add(new MailboxAddress(username, useremail));
                message.Subject = "Order Confirmation";
                message.Body = new TextPart("plain")
                {
                    Text = $"Hello, {username},\n\nThank you for your order from TheeArtSpace. " +
                           $"Your order for {productName} has been received. The total amount is ${amount}.\n\n" +
                           $"We appreciate your business!"
                };

                var smtpHost = _configuration["SmtpConfig:Host"];
                var smtpPort = int.Parse(_configuration["SmtpConfig:Port"]);
                var smtpUser = _configuration["SmtpConfig:Username"];
                var smtpPassword = _configuration["SmtpConfig:Password"];

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpHost, smtpPort, false);
                    await client.AuthenticateAsync(smtpUser, smtpPassword);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Consider logging the exception for better error handling
            }
        }


        internal async Task SendWelcomeMessage(UserDetailsDTO userDetails)
		{
			String useremail = userDetails.Email;
			String username = userDetails.Name;
			Console.WriteLine($"{useremail} {username}");
			try
			{
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress("TheeArtSpace", "mwanginancy333@gmail.com"));
				message.To.Add(new MailboxAddress(username, useremail));
				message.Subject = "WELCOME";
				message.Body = new TextPart("plain")
				{
					Text = $"Hello, {username},Welcome to  TheeArtSpace.Your account was created successfuly" +
					$"you can now proceed to login using the credentials you have just registered with."
				};


				var smtpHost = _configuration["SmtpConfig:Host"];
				var smtpPort = int.Parse(_configuration["SmtpConfig:Port"]);
				var smtpUser = _configuration["SmtpConfig:Username"];
				var smtpPassword = _configuration["SmtpConfig:Password"];

				using (var client = new SmtpClient())
				{
					client.Connect(smtpHost, smtpPort, false);
					client.Authenticate(smtpUser, smtpPassword);

					await client.SendAsync(message);
					client.Disconnect(true);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
