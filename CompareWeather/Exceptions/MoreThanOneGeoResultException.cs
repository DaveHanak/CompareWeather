using System;

namespace CompareWeather.Exceptions
{
    [Serializable]
    public class MoreThanOneGeoResultException : Exception
    {
        public MoreThanOneGeoResultException()
        { }

        public MoreThanOneGeoResultException(string message)
            : base(message)
        { }

        public MoreThanOneGeoResultException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
