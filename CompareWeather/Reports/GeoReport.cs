namespace CompareWeather.Reports
{
    public class GeoReport : Report
    {
        public string name { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string country { get; set; }
        public string state { get; set; }
    }
}
