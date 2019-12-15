using MapsAPIDemo.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MapsAPIDemo.Services
{
    public class MapsService : IMapsService
    {
        static string _googleMapsKey;
        private const string ApiBaseAddress = "https://maps.googleapis.com/maps/";

        private HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public async Task<SearchResultModel> GetTextSearch(SearchModel model)
        {
            var client = CreateClient();
            SearchResultModel resultModel = null;
            _googleMapsKey = "AIzaSyDf3-Pz5mWhY8gt9I-WGjJ3NGRtbnKHpfE";
            try
            {
                var response = await client.GetAsync($"api/place/findplacefromtext/json?" +
                    $"&input={model.Name}" +
                    $"&inputtype={model.InputType}" +
                    $"&fields={model.Fields}" +
                    $"&locationbias={model.LocationBias}" +
                    $"&key={_googleMapsKey}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    resultModel = SearchResultModel.FromJson(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return resultModel;
        }
    }
}