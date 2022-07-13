using System;

namespace OpenWeatherApplication.Models
{
    [Serializable]
    public class WeatherForecast
    {
        public string Location { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public string IconID { get; set; }
        public DateTime Date { get; set; }
        public string WindType { get; set; }
        public string WindDirection { get; set; }
        public double WindSpeed { get; set; }
        public double DayTemperature { get; set; }
        public double NightTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }               
    }
}
