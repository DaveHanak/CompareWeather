using CompareWeather.Exceptions;
using CompareWeather.Reports;
using CompareWeather.RequestHandlers;
using System;
using System.Linq;

namespace CompareWeather
{
    class Program
    {
        static void Main(string[] args)
        {
            RequestHandler inputData = new InputDataRequestHandler();
            RequestHandler weather = new WeatherRequestHandler();
            RequestHandler compare = new WeatherCompareHandler();
            inputData.SetSuccessor(weather);
            weather.SetSuccessor(compare);

            Console.WriteLine("Number of days:");
            var numberOfDays = int.Parse(Console.ReadLine());

            for (int i = 1; i <= 2; ++i)
            {
                Console.WriteLine($"City {i} name:");
                var cityName = Console.ReadLine();
                var request = new Request(numberOfDays, cityName);
                while (true)
                {
                    try
                    {
                        inputData.HandleRequest(request);
                        break;
                    }
                    catch (WrongNumberOfDaysException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Number of days:");
                        request.NumberOfDays = int.Parse(Console.ReadLine());
                    }
                    catch (MoreThanOneGeoResultException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine($"Latitude {i}:");
                        request.Latitude = float.Parse(Console.ReadLine());
                        Console.WriteLine($"Longitude {i}:");
                        request.Longitude = float.Parse(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine($"City {i} name:");
                        request.CityName = Console.ReadLine();
                    }
                }
            }

            var resultReports = compare.GetResults().Cast<WeatherCompareReport>().ToList();
            foreach (var report in resultReports)
            {
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine($"Day {report.Day}:\n");
                Console.WriteLine($"Higher average temperature:\n{report.HigherTemperatureCityName} -> {report.HigherTemperature} *C\tvs\t{report.LowerTemperatureCityName} -> {report.LowerTemperature} *C\n");
                Console.WriteLine($"Higher rain volume:\n{report.HigherRainVolumeCityName} -> {report.HigherRainVolume} mm/h\tvs\t{report.LowerRainVolumeCityName} -> {report.LowerRainVolume} mm/h\n");
            }
        }
    }
}
