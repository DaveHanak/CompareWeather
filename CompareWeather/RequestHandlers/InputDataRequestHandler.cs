using CompareWeather.Exceptions;
using CompareWeather.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace CompareWeather.RequestHandlers
{
    public class InputDataRequestHandler : RequestHandler
    {
        public override void HandleRequest(Request request)
        {
            if (!Enumerable.Range(1,5).Contains(request.NumberOfDays)) {
                throw new WrongNumberOfDaysException("Input data: Number of days should be within range 1 to 5.");
            }

            if (string.IsNullOrWhiteSpace(request.CityName))
            {
                throw new EmptyCityNameException("City name can't be empty.");
            }

            if (request.Latitude is null || request.Longitude is null)
            {
                var httpClient = new HttpClient();
                var json = httpClient.GetStringAsync(
                    $"http://api.openweathermap.org/geo/1.0/direct?q={request.CityName}&limit=5&appid={Constants.apiKey}")
                    .Result;
                var geoReports = JsonSerializer.Deserialize<List<GeoReport>>(json);
                if (geoReports.Count == 0)
                {
                    throw new ZeroGeoResultsException("No results found.");
                }
                else if (geoReports.Count > 1)
                {
                    string list = "Found locations:\n\n";
                    foreach (var geoReport in geoReports)
                    {
                        list += geoReport.name + ", " + geoReport.country + "\nat: " + geoReport.lat + ", " + geoReport.lon + "\n\n";
                    }
                    throw new MoreThanOneGeoResultException(list);
                }
                else
                {
                    var validGeoReport = geoReports.First();
                    request.Latitude = validGeoReport.lat;
                    request.Longitude = validGeoReport.lon;
                    request.CityName = validGeoReport.name + ", " + validGeoReport.country;
                }
            }
            else
            {
                var httpClient = new HttpClient();
                var json = httpClient.GetStringAsync(
                    $"http://api.openweathermap.org/geo/1.0/reverse?lat={request.Latitude}&lon={request.Longitude}&limit=1&appid={Constants.apiKey}")
                    .Result;
                var geoReport = JsonSerializer.Deserialize<List<GeoReport>>(json).First();
                request.CityName = geoReport.name + ", " + geoReport.country;
            }
            
            successor?.HandleRequest(request);
        }

        public override List<Report> GetResults()
        {
            return successor?.GetResults();
        }
    }
}
