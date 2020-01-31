using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Repair
{
    public class WeatherManager
    {
        private HttpClient _client;
        private readonly string _apiKey;

        private string _country = "UK";
        private string _city = "Wrexham";

        public WeatherManager(HttpClient client, string apiKey)
        {
            _apiKey = apiKey;
            _client = client;
        }

        public void SetCity(string city)
        {
            _city = city;
        }

        public void SetCountry(string country)
        {
            _country = country;
        }

        public WeatherInformation GetWeatherInformation()
        {
            try
            {
                var url = $"http://api.openweathermap.org/data/2.5/weather?q={_city},{_country}&APPID={_apiKey}";
                var response = _client.GetAsync(url);
                var jsonString = response.Result.Content.ReadAsStringAsync();
                jsonString.Wait();
                return JsonConvert.DeserializeObject<WeatherInformation>(jsonString.Result);
            }
            catch (Exception) // TODO test without internet, and maybe show error if weather cannot be gathered.
            {
                return WeatherInformation.Default;
            }
            
        }
        
    }
}