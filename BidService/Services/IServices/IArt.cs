using BidService.Models.Dtos;

namespace BidService.Services.IServices
{
    public interface IArt
    {
        Task<ArtDto> GetArtById(string Id);
    }
}
