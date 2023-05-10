using System.Text.Json.Serialization;

namespace CompareWeather.Reports
{
    public class WeatherReport : Report
    {
        public float lat { get; set; }
        public float lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public WeatherData[] data { get; set; }
    }

    public class WeatherData
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public float temp { get; set; }
        public float feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public float dew_point { get; set; }
        public float uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public float wind_speed { get; set; }
        public int wind_deg { get; set; }
        public Rain rain { get; set; }
        public Snow snow { get; set; }
    }

    public class Rain
    {
        [JsonPropertyName("1h")]
        public float oneh { get; set; }
    }

    public class Snow
    {
        [JsonPropertyName("1h")]
        public float oneh { get; set; }
    }
}
