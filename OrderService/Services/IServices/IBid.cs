using OrderService.Models.Dtos;

namespace OrderService.Services.IServices
{
    public interface IBid
    {
        Task<BidDto> GetBidById(string Id);
    }
}
