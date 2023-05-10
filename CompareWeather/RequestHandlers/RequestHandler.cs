using CompareWeather.Reports;
using System.Collections.Generic;

namespace CompareWeather.RequestHandlers
{
    public abstract class RequestHandler
    {
        protected RequestHandler successor;

        public void SetSuccessor(RequestHandler successor)
        {
            this.successor = successor;
        }

        public abstract void HandleRequest(Request request);

        public abstract List<Report> GetResults();
    }
}
