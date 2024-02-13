using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using OrderService.Data;
using OrderService.Models;
using System.Text;

namespace OrderService.Services
{
    public class AzureSender
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AzureSender(IConfiguration configuration)
        {
            _configuration=configuration;

        }
        public async Task PublishMessageToServiceBus(OrderAzure orderAzure)
        {
            try
            {
                string connectionString = "Endpoint=sb://socialapp.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=WueGKATfqCC3arSFG+mm3AQWfpzO39CD6+ASbJRa+OA=";
                string queueNameNow = "orders";


                var serviceBusConnectionString = _configuration.GetValue<string>("ServiceBus:ConnectionString");
                Console.WriteLine("hello world");

                var client = new QueueClient(connectionString, queueNameNow);

                var messageBody = JsonConvert.SerializeObject(orderAzure);
                var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(messageBody));

                await client.SendAsync(serviceBusMessage);
                Console.WriteLine("hello nancy");


                await client.CloseAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error publishing message to Service Bus: {ex.Message}");



            }
        }
    }
}