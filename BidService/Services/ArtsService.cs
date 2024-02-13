using BidService.Models.Dtos;
using BidService.Services.IServices;
using Newtonsoft.Json;

namespace BidService.Services
{
    public class ArtsService : IArt
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ArtsService(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;

        }
        public async Task<ArtDto> GetArtById(string Id)
        {
            var client = _httpClientFactory.CreateClient("Art");
            var response = await client.GetAsync(Id);
            var content = await response.Content.ReadAsStringAsync();
            var artDto = JsonConvert.DeserializeObject<ArtDto>(content);
            if (artDto != null && response.IsSuccessStatusCode)
            {
                return artDto;
            }
            return new ArtDto();
        }
    }
}
