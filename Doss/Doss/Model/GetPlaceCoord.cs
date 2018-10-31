using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Doss.Model2;
using Doss.Model_Forms_Place;
using System.IO;

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
            var result = GetRequestPlaceCooed<PlaceCoord>($"api/features/?text={x}%20{y}",x,y);
            return result;
        }
        private PlaceCoord GetRequestPlaceCooed<T>(string url, string x, string y)
        {
            try
            {             
                var response = _httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                PlaceCoord model = JsonConvert.DeserializeObject<PlaceCoord>(content);
                return model;
            }
            catch (Exception)
            {
                return GetRequestPlaceCooed<PlaceCoord>($"api/features/?text={x}%20{y}",x,y);
                //throw;
            }
           
        }

        public Place Get_Place(string cad_num)
        {
            var result = GetRequestPlace<Place>($"api/features/1/{cad_num}",cad_num);
            return result;
        }
        private Place GetRequestPlace<T>(string url,string cad_num)
        {
            try
            {
                var response = _httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                Place model = JsonConvert.DeserializeObject<Place>(content);
                return model;
            }
            catch (Exception)
            {

                return GetRequestPlace<T>($"api/features/1/{cad_num}",cad_num);
            }

        }

    }
}