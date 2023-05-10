using CompareWeather.Exceptions;
using CompareWeather.Reports;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace CompareWeather.RequestHandlers
{
    public class WeatherRequestHandler : RequestHandler
    {
        public override void HandleRequest(Request request)
        {
            if (request.Latitude is null || request.Longitude is null)
            {
                throw new ZeroGeoResultsException("Unresolved city coordinates.");
            }
            var httpClient = new HttpClient();
            var dateNow = DateTime.Now.ToUniversalTime();
            var dateOffset = new DateTimeOffset(new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 12, 0, 0));
            var dt = dateOffset.ToUnixTimeSeconds();
            var weatherReports = new List<WeatherReport>();
            for (int i = 0; i < request.NumberOfDays; ++i)
            {
                float lat = (float)request.Latitude;
                float lon = (float)request.Longitude;
                var json = httpClient.GetStringAsync(
                    $"https://api.openweathermap.org/data/3.0/onecall/timemachine?lat={lat}&lon={lon}&dt={dt}&units=metric&appid={Constants.apiKey}")
                    .Result;
                weatherReports.Add(JsonSerializer.Deserialize<WeatherReport>(json));
                dt -= 86400;
            }

            foreach (var weatherReport in weatherReports)
            {
                var countTemperatures = 0f;
                var sumTemperatures = 0f;
                var sumRain = 0f;
                foreach (var data in weatherReport.data)
                {
                    countTemperatures++;
                    sumTemperatures += data.temp;
                    if (data.rain != null)
                    {
                        sumRain += data.rain.oneh;
                    }
                }
                request.AvgTemps.Add(sumTemperatures / countTemperatures);
                request.RainVols.Add(sumRain);
            }

            successor?.HandleRequest(request);
        }

        public override List<Report> GetResults()
        {
            return successor?.GetResults();
        }
    }
}
