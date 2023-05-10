using System;

namespace CompareWeather.Exceptions
{
    [Serializable]
    public class WrongNumberOfDaysException : Exception
    {
        public WrongNumberOfDaysException()
        { }

        public WrongNumberOfDaysException(string message)
            : base(message)
        { }

        public WrongNumberOfDaysException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
