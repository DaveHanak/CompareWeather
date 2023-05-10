using CompareWeather.Exceptions;
using CompareWeather.Reports;
using System.Collections.Generic;
using System.Linq;

namespace CompareWeather.RequestHandlers
{
    public class WeatherCompareHandler : RequestHandler
    {
        private Request requestA;
        private Request requestB;
        private readonly List<Report> reports = new();

        public override void HandleRequest(Request request)
        {
            if (!Enumerable.Range(1, 5).Contains(request.NumberOfDays))
            {
                throw new WrongNumberOfDaysException("Compare weather: Number of days should be within range 1 to 5 for both requests.");
            }

            if (requestA is null)
            {
                requestA = request;
                return;
            }

            if (requestB is null)
            {
                requestB = request;
            }

            if (requestA.NumberOfDays != requestB.NumberOfDays)
            {
                requestA = null;
                requestB = null;
                throw new WrongNumberOfDaysException("Compare weather: Number of days should be the same for both requests. Setting both requests to null...");
            }

            for (int i = 0; i < requestA.NumberOfDays; ++i)
            {
                bool HigherTemperatureA = requestA.AvgTemps[i] > requestB.AvgTemps[i];
                bool HigherRainVolumeA = requestA.RainVols[i] > requestB.RainVols[i];
                var report = new WeatherCompareReport
                {
                    Day = i + 1,

                    HigherTemperatureCityName =
                    HigherTemperatureA ?
                    requestA.CityName :
                    requestB.CityName,

                    LowerTemperatureCityName =
                    !HigherTemperatureA ?
                    requestA.CityName :
                    requestB.CityName,

                    HigherTemperature = 
                    HigherTemperatureA ?
                    requestA.AvgTemps[i] :
                    requestB.AvgTemps[i],

                    LowerTemperature =
                    !HigherTemperatureA ?
                    requestA.AvgTemps[i] :
                    requestB.AvgTemps[i],

                    HigherRainVolumeCityName =
                    HigherRainVolumeA ?
                    requestA.CityName :
                    requestB.CityName,

                    LowerRainVolumeCityName =
                    !HigherRainVolumeA ?
                    requestA.CityName :
                    requestB.CityName,

                    HigherRainVolume =
                    HigherRainVolumeA ?
                    requestA.RainVols[i] :
                    requestB.RainVols[i],

                    LowerRainVolume =
                    !HigherRainVolumeA ?
                    requestA.RainVols[i] :
                    requestB.RainVols[i]
                };

                reports.Add(report);
            }
        }

        public override List<Report> GetResults()
        {
            return reports;
        }
    }
}
