using System.Collections.Generic;

namespace CompareWeather
{
    public class Request
    {
        public int NumberOfDays { get; set; }
        public string CityName { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public List<float> AvgTemps { get; set; }
        public List<float> RainVols { get; set; }

        public Request(int numberOfDays, string cityName)
        {
            NumberOfDays = numberOfDays;
            CityName = cityName;

            AvgTemps = new();
            RainVols = new();
        }
    }
}
