using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompareWeather.RequestHandlers;
using CompareWeather.Exceptions;
using CompareWeather;
using System.Linq;

namespace CompareWeatherTests
{
    [TestClass]
    public class CompareWeatherTests
    {
        [TestMethod]
        public void HandleGeoRequest()
        {
            var geoRequestHandler = new InputDataRequestHandler();
            var request = new Request(3, "Valetta,MT");
            try
            {
                geoRequestHandler.HandleRequest(request);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void HandleNumberOfDaysOutOfRange()
        {
            var geoRequestHandler = new InputDataRequestHandler();
            var request = new Request(0, "Berlin,DE");
            _ = Assert.ThrowsException<WrongNumberOfDaysException>(() => geoRequestHandler.HandleRequest(request));
        }

        [TestMethod]
        public void HandleCityNameEmpty()
        {
            var geoRequestHandler = new InputDataRequestHandler();
            var request = new Request(1, "");
            _ = Assert.ThrowsException<EmptyCityNameException>(() => geoRequestHandler.HandleRequest(request));
        }

        [TestMethod]
        public void HandleCityNameNull()
        {
            var geoRequestHandler = new InputDataRequestHandler();
            var request = new Request(1, null);
            _ = Assert.ThrowsException<EmptyCityNameException>(() => geoRequestHandler.HandleRequest(request));
        }

        [TestMethod]
        public void HandleCityNameAllWhitespaces()
        {
            var geoRequestHandler = new InputDataRequestHandler();
            var request = new Request(1, "    \t\n    ");
            _ = Assert.ThrowsException<EmptyCityNameException>(() => geoRequestHandler.HandleRequest(request));
        }

        [TestMethod]
        public void HandleCityNameShouldTrimString()
        {
            var geoRequestHandler = new InputDataRequestHandler();
            var request = new Request(1, "    \t\n    Berlin,DE   ");
            try
            {
                geoRequestHandler.HandleRequest(request);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void HandleCityNameMoreThanOneResult()
        {
            var geoRequestHandler = new InputDataRequestHandler();
            var request = new Request(1, "Berlin");
            try
            {
                geoRequestHandler.HandleRequest(request);
                Assert.Fail();
            }
            catch (MoreThanOneGeoResultException)
            {
            }
        }

        [TestMethod]
        public void HandleWeatherRequest()
        {
            var weatherRequestHandler = new WeatherRequestHandler();
            var request = new Request(3, "Berlin")
            {
                Longitude = 52.5170365f,
                Latitude = 13.3888599f
            };
            try
            {
                weatherRequestHandler.HandleRequest(request);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void HandleWeatherRequestNullCoordinates()
        {
            var weatherRequestHandler = new WeatherRequestHandler();
            var request = new Request(1, "Berlin");
            try
            {
                weatherRequestHandler.HandleRequest(request);
                Assert.Fail();
            }
            catch (ZeroGeoResultsException)
            {
            }
        }

        [TestMethod]
        public void HandleWeatherCompare()
        {
            var weatherCompareHandler = new WeatherCompareHandler();
            var requestA = new Request(5, "City A")
            {
                AvgTemps =
                {
                    10.0f,
                    11.0f,
                    12.0f,
                    13.0f,
                    14.0f
                },
                RainVols =
                {
                    2.0f,
                    2.1f,
                    2.2f,
                    2.3f,
                    2.4f
                }
            };
            var requestB = new Request(5, "City B")
            {
                AvgTemps =
                {
                    20.0f,
                    21.0f,
                    22.0f,
                    23.0f,
                    24.0f
                },
                RainVols =
                {
                    1.0f,
                    1.1f,
                    1.2f,
                    1.3f,
                    1.4f
                }
            };
            try
            {
                weatherCompareHandler.HandleRequest(requestA);
                weatherCompareHandler.HandleRequest(requestB);

                var resultReports = weatherCompareHandler.GetResults().Cast<CompareWeather.Reports.WeatherCompareReport>().ToList();
                foreach (var report in resultReports)
                {
                    Assert.IsTrue(report.HigherTemperatureCityName.Contains("B"));
                    Assert.IsTrue(report.LowerTemperatureCityName.Contains("A"));
                    Assert.IsTrue(report.HigherRainVolumeCityName.Contains("A"));
                    Assert.IsTrue(report.LowerRainVolumeCityName.Contains("B"));
                    Assert.IsTrue(report.HigherTemperature > report.LowerTemperature);
                    Assert.IsTrue(report.HigherRainVolume > report.LowerRainVolume);
                }
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void HandleWeatherCompareDifferentNumberOfDays()
        {
            var weatherCompareHandler = new WeatherCompareHandler();
            var requestA = new Request(4, "City A")
            {
                AvgTemps =
                {
                    10.0f,
                    11.0f,
                    12.0f,
                    13.0f
                },
                RainVols =
                {
                    2.0f,
                    2.1f,
                    2.2f,
                    2.3f
                }
            };
            var requestB = new Request(5, "City B")
            {
                AvgTemps =
                {
                    20.0f,
                    21.0f,
                    22.0f,
                    23.0f,
                    24.0f
                },
                RainVols =
                {
                    1.0f,
                    1.1f,
                    1.2f,
                    1.3f,
                    1.4f
                }
            };
            try
            {
                weatherCompareHandler.HandleRequest(requestA);
                weatherCompareHandler.HandleRequest(requestB);
                Assert.Fail();
            }
            catch (WrongNumberOfDaysException)
            {
            }
        }

        [TestMethod]
        public void HandleWeatherCompareNumberOfDaysOutOfRange()
        {
            var weatherCompareHandler = new WeatherCompareHandler();
            var requestA = new Request(0, "City A");
            try
            {
                weatherCompareHandler.HandleRequest(requestA);
                Assert.Fail();
            }
            catch (WrongNumberOfDaysException)
            {
            }
        }
    }
}
