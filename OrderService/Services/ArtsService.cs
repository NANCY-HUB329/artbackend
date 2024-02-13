using Newtonsoft.Json;
using OrderService.Models.Dtos;
using OrderService.Services.IServices;

namespace OrderService.Services
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
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto.Result != null && response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ArtDto>(responseDto.Result.ToString());
            }
            return new ArtDto();
        }

    }
}
