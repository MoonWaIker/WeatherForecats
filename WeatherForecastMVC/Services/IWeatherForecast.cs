using WeatherForecastMVC.Models;

namespace WeatherForecastMVC.Services
{
    public interface IWeatherForecast
    {
        public WeatherForecastModel GetWeatherForecast(string city);
    }
}