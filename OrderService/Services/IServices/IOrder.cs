using OrderService.Models;
using OrderService.Models.Dtos;

namespace OrderService.Services.IServices
{
    public interface IOrder
    {
        Task<string> PlaceOrder(Orders order, string token);
        Task saveChanges();
        Task<List<Orders>> GetAllOrders(Guid userId);
        Task<Orders> GetOrderById(Guid Id);
        Task<StripeRequestDto> MakePayments(StripeRequestDto stripeRequestDto, string token);
        Task<bool> ValidatePayments(Guid OrderId);
    }
}
