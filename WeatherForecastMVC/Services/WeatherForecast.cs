using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WeatherForecastMVC.Models;

namespace WeatherForecastMVC.Services
{
    public class WeatherForecast : IWeatherForecast, IDisposable
    {
        private const string apiKey = "5oGXEEGsi6pfKhbk2Ai4tNWUpMObG9H9";

        private readonly RestClient client = new("https://dataservice.accuweather.com");

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
        }

        public WeatherForecastModel GetWeatherForecast(string city)
        {
            string locationKey = GetCityKey(city);

            RestRequest request = new RestRequest($"/forecasts/v1/daily/1day/{locationKey}", Method.Get)
            .AddParameter("apikey", apiKey)
            .AddParameter("metric", true);
            RestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new HttpRequestException();
            }

            WeatherForecastModel model = JObject.Parse(response.Content ?? throw new JsonSerializationException())["DailyForecasts"]?[0]?
            .ToObject<WeatherForecastModel>() ?? throw new JsonSerializationException();
            model.City = city;

            return model;
        }

        private string GetCityKey(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                throw new ArgumentNullException(nameof(city));
            }

            RestRequest request = new RestRequest("/locations/v1/cities/search", Method.Get)
            .AddParameter("apikey", apiKey)
            .AddParameter("q", city);
            RestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new HttpRequestException();
            }

            return (JArray.Parse(response.Content!)[0]["Key"] ?? throw new ArgumentException("Response is empty. Maybe city is incorrect", nameof(city))).ToString();
        }
    }
}