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
            LowestTemperature = (double)(additionalData?["Temperature"]?["Minimum"]?["Value"] ?? throw new JsonSerializationException());

            HighestTemperature = (double)(additionalData["Temperature"]?["Maximum"]?["Value"] ?? throw new JsonSerializationException());

            if ((bool)(additionalData["Day"]["HasPrecipitation"] ?? throw new JsonSerializationException()) || (bool)(additionalData["Night"]["HasPrecipitation"] ?? throw new JsonSerializationException()))
            {
                Precipitation = (bool)additionalData["Day"]["HasPrecipitation"]!
                    ? $"{(string)additionalData["Day"]["PrecipitationIntensity"]!} {(string)additionalData["Day"]["PrecipitationType"]!}"
                    : $"{(string)additionalData["Night"]["PrecipitationIntensity"]!} {(string)additionalData["Night"]["PrecipitationType"]!}";
            }
        }
    }
}