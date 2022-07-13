using Microsoft.Practices.Unity;
using OpenWeatherApplication.Services;
using OpenWeatherApplication.ViewModels;

namespace OpenWeatherApplication.Utils
{
    public class ViewModelLocator
    {
        private UnityContainer _container;
        public ViewModelLocator()
        {
            _container = new UnityContainer();
            _container.RegisterType<IWeatherService, OpenWeatherMapService>();
            _container.RegisterType<IDialogService, DialogService>();
        }

        public WeatherViewModel WeatherVM
        {
            get { return _container.Resolve<WeatherViewModel>(); }
        }
    }
}
