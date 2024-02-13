using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using EmailService.Models;
using OrderService.Models;

namespace EmailService.Services
{
    public class AzureServices : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly MailService _mailService;

        public AzureServices(IConfiguration configuration, MailService mailService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SendAuthEmail(stoppingToken);
            //await OrderService(stoppingToken);
        }

        public async Task SendAuthEmail(CancellationToken stoppingToken)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:AzureServiceBusConnection").Value;
            string queueName = _configuration.GetSection("ServiceBus:register").Value;

            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                ServiceBusReceiver receiver = client.CreateReceiver(queueName);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

                        if (receivedMessage != null)
                        {
                            string messageBody = Encoding.UTF8.GetString(receivedMessage.Body);
                            Console.WriteLine($"Received a message from the queue: {messageBody}");

                            await receiver.CompleteMessageAsync(receivedMessage);
                            UserDetailsDTO? userDetails = GetUsernameEmail(messageBody);

                            if (userDetails != null)
                            {
                                await _mailService.SendWelcomeMessage(userDetails);
                            }
                            else
                            {
                                // Log or handle the case where userDetails is null
                            }
                        }
                        else
                        {
                            Console.WriteLine("No messages available in the queue. Waiting for new messages...");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        Console.WriteLine($"Error receiving message: {ex.Message}");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        public async Task OrderService(CancellationToken stoppingToken)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:AzureServiceBusConnection").Value;
            string queueName = _configuration.GetSection("ServiceBus:orders").Value;

            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                ServiceBusReceiver receiver = client.CreateReceiver(queueName);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

                        if (receivedMessage != null)
                        {
                            string messageBody = Encoding.UTF8.GetString(receivedMessage.Body);
                            Console.WriteLine($"Received a message from the queue: {messageBody}");

                            await receiver.CompleteMessageAsync(receivedMessage);
                            OrderAzure? orderAzure = GetOrderDto(messageBody);

                            if (orderAzure != null)
                            {
                                await _mailService.SendOrderConfirmation(orderAzure);
                            }
                            else
                            {
                                // Log or handle the case where orderAzure is null
                            }
                        }
                        else
                        {
                            Console.WriteLine("No messages available in the queue. Waiting for new messages...");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        Console.WriteLine($"Error receiving message: {ex.Message}");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        public UserDetailsDTO? GetUsernameEmail(string userDetails)
        {
            try
            {
                return JsonConvert.DeserializeObject<UserDetailsDTO>(userDetails);
            }
            catch (Exception e)
            {
                // Log the exception
                Console.WriteLine($"Error deserializing user details: {e.Message}");
                return null;
            }
        }

        public OrderAzure? GetOrderDto(string orderAzure)
        {
            try
            {
                return JsonConvert.DeserializeObject<OrderAzure>(orderAzure);
            }
            catch (Exception e)
            {
                // Log the exception
                Console.WriteLine($"Error deserializing order details: {e.Message}");
                return null;
            }
        }
    }
}
