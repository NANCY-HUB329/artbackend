using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentications.Services.AuthService.Services;
using Microsoft.EntityFrameworkCore;
using OrderService.Data; // Add the appropriate namespace for your ApplicationDbContext
using OrderService.Models; // Add the appropriate namespace for your Orders model
using OrderService.Models.Dtos;
using OrderService.Services.IServices;
using Stripe;
using Stripe.Checkout;
using Stripe.Issuing;

namespace OrderService.Services
{
    public class OrdersService : IOrder
    {
        private readonly ApplicationDbContext _context;
        private readonly IUser _userService;
        private readonly IBid _bidService;
        private readonly JwtService _jwtService; // Corrected the variable name
        private readonly AzureSender _azureSender;
        //private readonly IMessageBus _messageBUs;

        public OrdersService(IUser userService, IBid bidService, ApplicationDbContext context, JwtService jwtService, AzureSender azureSender)
        {
            _userService = userService;
            _context = context;
            _bidService = bidService;
            _jwtService = jwtService; // Corrected the variable assignment
            _azureSender = azureSender;

        }

        public async Task<List<Orders>> GetAllOrders(Guid userId)
        {
            return await _context.Orders.Where(b => b.BidderId == userId).ToListAsync();
        }

        public async Task<Orders> GetOrderById(Guid Id)
        {
            return await _context.Orders.Where(b => b.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<StripeRequestDto> MakePayments(StripeRequestDto stripeRequestDto,string token)
        {

            var order = await _context.Orders.Where(x => x.Id == stripeRequestDto.OrderId).FirstOrDefaultAsync();
            var bid = await _bidService.GetBidById(order.BidId.ToString());

            string userEmail = _jwtService.GetEmailFromToken(token);
            OrderAzure orderAzure = new OrderAzure()
            {
                artName = "gjbsfhkbgskdfgj",
                email = userEmail,
                amount = order.TotalAmount



            };
            _azureSender.PublishMessageToServiceBus(orderAzure);






            var options = new SessionCreateOptions()
            {
                SuccessUrl = stripeRequestDto.ApprovedUrl,
                CancelUrl = stripeRequestDto.CancelUrl,
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>()
            };

            var item = new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmount = (long)order.TotalAmount * 100,
                    Currency = "kes",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = bid.ArtName,
                        Images = new List<string> { "https://imgs.search.brave.com/av4uh1BAXrv7q2gkJt-E709vrIz3mB1-wrcPDtDyZNI/rs:fit:500:0:0/g:ce/aHR0cHM6Ly93d3cu/ZXhwZXJ0YWZyaWNh/LmNvbS9pbWFnZXMv/YXJlYS8xODI5X2wu/anBn" }
                    }
                },
                Quantity = 1
            };

            options.LineItems.Add(item);

            var service = new SessionService();
            Session session = service.Create(options);

            stripeRequestDto.StripeSessionUrl = session.Url;
            stripeRequestDto.StripeSessionId = session.Id;

            order.StripeSessionId = session.Id;
            order.Status = "Ongoing";
            await _context.SaveChangesAsync();

            return stripeRequestDto;
        }

        public async Task<string> PlaceOrder(Orders order, string token)
        {
            Console.WriteLine("leon");
        

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return "Order Placed Successfully";
        }

        public Task saveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidatePayments(Guid OrderId)
        {
            throw new NotImplementedException();
        }
    }
}
