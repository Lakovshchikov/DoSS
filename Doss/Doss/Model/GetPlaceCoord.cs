using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Doss.Model2;

namespace Doss.Model
{
    class GetPlace
    {
        private readonly HttpClient _httpClient;
        public GetPlace()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://pkk5.rosreestr.ru")
            };
        }

        public PlaceCoord GetPlaceCoordMethod(string x, string y)
        {
            var result = GetRequestPlaceCooed<PlaceCoord>($"api/features/?text={x}%20{y}");
            return result;
        }
        private PlaceCoord GetRequestPlaceCooed<T>(string url)
        {
            var response =  _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            var content =  response.Content.ReadAsStringAsync().Result;
            PlaceCoord model = JsonConvert.DeserializeObject<PlaceCoord>(content);
            return model;
        }

        public Place Get_Place(string kad_num)
        {
            var result = GetRequestPlace<Place>($"api/features/1/{kad_num}");
            return result;
        }
        private Place GetRequestPlace<T>(string url)
        {
            var response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            Place model = JsonConvert.DeserializeObject<Place>(content);
            return model;
        }
    }
}
//private async Task<T> GetRequest<T>(string url)
//{
//    var response = await _httpClient.GetAsync(url);
//    response.EnsureSuccessStatusCode();
//    var content = await response.Content.ReadAsStringAsync();
//    var model = JsonConvert.DeserializeObject<T>(content);
//    return model;
//}

//public async Task<PlaceCoord> GetPlaceCoordMethod(string x, string y)
//{
//    var result = await GetRequest<PlaceCoord>($"api/features/?text={x}%20{y}");
//    return result;
//}