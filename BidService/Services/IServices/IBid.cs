using BidService.Models;

namespace BidService.Services.IServices
{
    public interface IBid
    {
        Task<string> AddBid(Bid bid);
        Task<List<Bid>> GetArBids(Guid ArtId);
        Task <List <Bid>> HighestBidsPerItem(Guid userId);
        Task<List<Bid>> GetMyBids(Guid userId);
        Task<Bid> GetOneBid(Guid Id);
        Task<List<Bid>> GetMyWins(Guid userId);

        //Task<string> UpdateBid();
        Task<string> DeleteBid(Bid art);
    }
}
