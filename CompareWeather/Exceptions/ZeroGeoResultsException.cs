using System;

namespace CompareWeather.Exceptions
{
    [Serializable]
    public class ZeroGeoResultsException : Exception
    {
        public ZeroGeoResultsException()
        { }

        public ZeroGeoResultsException(string message)
            : base(message)
        { }

        public ZeroGeoResultsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
