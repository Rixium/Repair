namespace Repair
{
    public class WeatherInformation
    {

        // TODO DEFAULT WEATHER INFORMATION
        public static WeatherInformation Default = new WeatherInformation()
        {
            Weather = new WeatherData[]
            {
                new WeatherData()
                {
                }
            },
            Wind = new WindData()
            {
                
            }
        };
        
        public WeatherData[] Weather { get; set; }
        public WindData Wind { get; set; }
    }
}