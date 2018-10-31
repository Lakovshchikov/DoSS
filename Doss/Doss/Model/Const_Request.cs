using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Doss.Model_Forms_Place;
using Doss.Model_Place_Categories;
using Doss.Model_Type_Of_Use;
using System.IO;

namespace Doss.Model
{
    class Const_Request
    {
        private readonly HttpClient _httpClient;
        public Const_Request()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://pkk5.rosreestr.ru")
            };
        }

        public Form_Place GetRequestFormPlaceMethod()
        {
            var result = GetRequestFormPlace<Form_Place>($"arcgis/rest/services/Cadastre/Thematic/MapServer/1?f=pjson");
            return result;
        }
        private Form_Place GetRequestFormPlace<T>(string url)
        {
            try
            {
                var response = _httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                Form_Place model = JsonConvert.DeserializeObject<Form_Place>(content);
                return model;
            }
            catch (Exception)
            {
                return GetRequestFormPlace<Form_Place>($"arcgis/rest/services/Cadastre/Thematic/MapServer/1?f=pjson");
                
            }
            
        }
        public Place_Categories GetRequestPlaceCategoriesMethod()
        {
            var result = GetRequestPlaceCategories<Place_Categories>($"arcgis/rest/services/Cadastre/Thematic/MapServer/14?f=pjson");
            return result;
        }
        private Place_Categories GetRequestPlaceCategories<T>(string url)
        {
            try
            {
                var response = _httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                Place_Categories model = JsonConvert.DeserializeObject<Place_Categories>(content);
                return model;
            }
            catch (Exception)
            {
               return GetRequestPlaceCategories<T>($"arcgis/rest/services/Cadastre/Thematic/MapServer/14?f=pjson");
                
            }
            
        }
        public TypesOf_Use GetRequestTypesOf_UseMethod()
        {
            var result = GetRequestTypesOf_Use<TypesOf_Use>($"arcgis/rest/services/Cadastre/Thematic/MapServer/6?f=pjson");
            return result;
        }
        private TypesOf_Use GetRequestTypesOf_Use<T>(string url)
        {
            try
            {
                var response = _httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                TypesOf_Use model = JsonConvert.DeserializeObject<TypesOf_Use>(content);
                return model;
            }
            catch (Exception)
            {
               return GetRequestTypesOf_Use<TypesOf_Use>($"arcgis/rest/services/Cadastre/Thematic/MapServer/6?f=pjson");
               
            }
            
        }
    }
}
