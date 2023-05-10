using System;

namespace CompareWeather.Exceptions
{
    [Serializable]
    public class EmptyCityNameException : Exception
    {
        public EmptyCityNameException()
        { }

        public EmptyCityNameException(string message)
            : base(message)
        { }

        public EmptyCityNameException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
