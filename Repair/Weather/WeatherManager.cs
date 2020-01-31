namespace Repair
{
    public class WeatherManager
    {
        private readonly string _apiKey;

        private string _country = "UK";
        private string _city = "Wrexham";

        public WeatherManager(string apiKey)
        {
            _apiKey = apiKey;
        }

        public void SetCity(string city)
        {
            _city = city;
        }

        public void SetCountry(string country)
        {
            _country = country;
        }
        
    }
}