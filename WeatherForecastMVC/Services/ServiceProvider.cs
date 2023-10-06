namespace WeatherForecastMVC.Services
{
    public static class ServiceProviderExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            _ = services.AddTransient(typeof(IWeatherForecast), typeof(WeatherForecast));
        }
    }
}