using System.Collections.Generic;
using OpenWeatherApplication.Models;
using System.Threading.Tasks;

namespace OpenWeatherApplication.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetForecastAsync(string location, int days);
    }
}
