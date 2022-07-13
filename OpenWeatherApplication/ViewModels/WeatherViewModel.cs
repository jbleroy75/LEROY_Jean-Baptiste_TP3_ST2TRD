using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OpenWeatherApplication.Utils;
using OpenWeatherApplication.Commands;
using OpenWeatherApplication.Services;
using OpenWeatherApplication.Models;
using System.Net.Http;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;

namespace OpenWeatherApplication.ViewModels
{
    public class WeatherViewModel : ViewModelBase
    {
        private IWeatherService weatherService;
        private IDialogService dialogService;

        public WeatherViewModel(IWeatherService ws, IDialogService ds)
        {
            weatherService = ws;
            dialogService = ds;
        }

        private List<WeatherForecast> _forecastToSave;
        public List<WeatherForecast> ForecastToSave
        {
            get { return _forecastToSave; }
            set
            {
                _forecastToSave = value;
                OnPropertyChanged();
            }
        }
        private List<WeatherForecast> _forecast;
        public List<WeatherForecast> Forecast
        {
            get { return _forecast; }
            set
            {
                _forecast = value;
                OnPropertyChanged();
            }
        }

        private WeatherForecast _currentWeather = new WeatherForecast();
        public WeatherForecast CurrentWeather
        {
            get { return _currentWeather; }
            set
            {
                _currentWeather = value;
                OnPropertyChanged();
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        private ICommand _getWeatherCommand;
        public ICommand GetWeatherCommand
        {
            get
            {
                if (_getWeatherCommand == null) _getWeatherCommand = 
                        new RelayCommandAsync(() => GetWeather(), (o) => CanGetWeather());               
                return _getWeatherCommand;
            }
        }
        private ICommand _saveDataCommand;
        public ICommand SaveDataCommand
        {
            get
            {
                if (_saveDataCommand == null) _saveDataCommand = 
                        new RelayCommand((o) => SaveWeatherData(), (o) => true);               
                return _saveDataCommand;
            }
        }
        public void SaveWeatherData(object obj = null)
        {
            if(ForecastToSave != null && ForecastToSave.Count > 0)
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.ShowDialog();
                FileStream fs = new FileStream(fileDialog.FileName + ".bin", FileMode.Create);
                try
                {
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    binFormatter.Serialize(fs, ForecastToSave);
                    fs.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error Occurred in Creating Binary File");
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }

                MessageBox.Show("File Created Successfull");
            }
            else
                MessageBox.Show("No Data to export..!");

        }
        public async Task GetWeather()
        {
            try
            {
                var weather = await weatherService.GetForecastAsync(Location, 3);
                CurrentWeather = weather.First();
                Forecast = new List<WeatherForecast>();
                foreach (var w in weather)
                {
                    if (!Forecast.Any(x => x.Date.Date == w.Date.Date))
                    {
                        Forecast.Add(w);
                    }
                }
                //Forecast = weather.OrderBy(x => x.Date.Date).Distinct().ToList();
                //Forecast = weather.Skip(1).Take(6).ToList();
                ForecastToSave = new List<WeatherForecast>(Forecast);
                Forecast = Forecast.Skip(1).Take(6).ToList();
            }
            catch (HttpRequestException ex) {
                dialogService.ShowNotification("Ensure you're connected to the internet!", "Open Weather");
            }
            catch (LocationNotFoundException ex)
            {
                dialogService.ShowNotification("Location not found!", "Open Weather");
            }
            
        }

        private ICommand _loadDataCommand;
        public ICommand LoadDataCommand
        {
            get
            {
                if (_loadDataCommand == null) _loadDataCommand =
                        new RelayCommand((o) => LoadWeatherDataFromFile(), (o) => true);
                return _loadDataCommand;
            }
        }
        private void LoadWeatherDataFromFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            string fileName = fileDialog.FileName;
            BinaryFormatter binFormatter = new BinaryFormatter();

            if (File.Exists(fileName))
            {


                FileStream readerFileStream = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read);
                try
                {
                    ForecastToSave = new List<WeatherForecast>();
                    ForecastToSave = (List<WeatherForecast>)binFormatter.Deserialize(readerFileStream);

                    CurrentWeather = ForecastToSave.First();
                    Forecast = ForecastToSave.Skip(1).Take(6).ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occurred in Reading Binary File");
                }
                finally
                {
                    if (readerFileStream != null)
                        readerFileStream.Close();
                }
            }
        }
        public Boolean CanGetWeather()
        {
            return Location != string.Empty;
        }
    }
}
