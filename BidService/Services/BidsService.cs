using BidService.Data;
using BidService.Models;
using BidService.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace BidService.Services
{
    public class BidsService : IBid
    {
        private readonly ApplicationDbContext _context;
        public BidsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddBid(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
            return "Bid was successful";
        }

        public async Task<List<Bid>> GetArBids(Guid ArtId)
        {
          return  await _context.Bids.Where(b=>b.ArtId== ArtId).ToListAsync();
        }

        public  async Task<List<Bid>> GetMyBids(Guid userId)
        {
            return await _context.Bids.Where(b => b.BidderId == userId).ToListAsync();
        }

        public async Task<List<Bid>> GetMyWins(Guid userId)
        {
            var MyWins = await _context.Bids
            .Where(b => b.BidderId == userId)
            .GroupBy(b => b.ArtId)
            .Select(group =>
                group.OrderByDescending(b => b.BidAmmount)
                     .FirstOrDefault(b => b.BidderId == group.OrderByDescending(x => x.BidAmmount).First().BidderId))
            .ToListAsync();

            return MyWins;

        }

        public async Task<Bid> GetOneBid(Guid Id)
        {
            return await _context.Bids.Where(b=>b.Id== Id).FirstOrDefaultAsync();    
        }

        public async Task<List<Bid>> HighestBidsPerItem(Guid userId)
        {
            var highestBids = await _context.Bids
                .Where(b => b.BidderId == userId)
                .GroupBy(b => b.ArtId)
                .Select(g => g.OrderByDescending(b => b.BidAmmount).FirstOrDefault()) // Select the highest bid for each ArtId
                .ToListAsync();

            return highestBids;
        }
        public async Task<string> DeleteBid(Bid art)
        {
            _context.Bids.Remove(art);
            await _context.SaveChangesAsync();
            return "Bid removed successfully";
        }
    }
}
