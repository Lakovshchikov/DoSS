using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Doss.Model
{
    class GetPlaceCoord
    {
        private readonly HttpClient _httpClient;
        public GetPlaceCoord()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://pkk5.rosreestr.ru")
            };
        }

        public async Task<PlaceCoord> GetPlaceCoordMethod(string x, string y)
        {
            var result = await GetRequest<PlaceCoord>($"api/features/?text={x}%20{y}");
            return result;
        }
        private async Task<T> GetRequest<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<T>(content);
            return model;
        }
    }
}
