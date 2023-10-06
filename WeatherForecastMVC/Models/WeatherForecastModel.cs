using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherForecastMVC.Models
{
    public class WeatherForecastModel
    {
        public string City { get; set; } = string.Empty;

        public string Precipitation { get; set; } = string.Empty;

        public double LowestTemperature { get; set; }

        public double HighestTemperature { get; set; }

        [JsonExtensionData]
        protected IDictionary<string, JToken>? additionalData;

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext context)
        {
            LowestTemperature = (double)(additionalData?["Minimum"]?["Value"] ?? throw new JsonSerializationException());

            HighestTemperature = (double)(additionalData["Maximum"]?["Value"] ?? throw new JsonSerializationException());
        }
    }
}