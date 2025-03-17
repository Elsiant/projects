using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MyWidgets.Commands;
using MyWidgets.Common;
using MyWidgets.Models;
using Newtonsoft.Json.Linq;

namespace MyWidgets.ViewModels
{
    public class WeatherViewModel : ViewModelBase
    {
        private static readonly string API_KEY = "b043c91ecaf24a3abbcdeacddab91d7e";    // API Key 추후 이동

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private readonly HttpClientService _httpClientService;
        private WeatherModel _model = new WeatherModel();        


        #region ViewModel 정의
        public string Icon
        {
            get => _model?.Icon ?? "/Resources/icon_x.png";
            set
            {
                _model.Icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
        public string CityName
        {
            get => _model?.CityName ?? "Unknown";
            set
            {
                _model.CityName = value;
                OnPropertyChanged(nameof(CityName));
            }
        }

        public string Description
        {
            get => _model?.Description ?? "Unknown";
            set
            {
                _model.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Temperature
        {
            get => _model?.Temperature ?? "0";
            set
            {
                _model.Temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

        public string Humidity
        {
            get => _model?.Humidity ?? "0";
            set
            {
                _model.Humidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }

        public string WeatherType
        {
            get => _model?.WeatherType ?? "0";
            set
            {
                _model.WeatherType = value;
                OnPropertyChanged(nameof(WeatherType));
            }
        }
        #endregion

        #region Command 정의
        public ICommand CommandLoad { get; set; }

        public void Onload(object parameter)
        {
            GetWeatherInfo();
        }
        #endregion

        public WeatherViewModel()
        {
            // 디자이너용 생성자
            CityName = "Seoul";
            Description = "Clear";
            Temperature = "25";
            Humidity = "50";
            WeatherType = "Clear";
            Icon = $"/Resources/icon_{WeatherType}.png".ToLower();
        }
        public WeatherViewModel(HttpClientService httpClientService)
        {
            //임시 코드
            CityName = "Seoul";

            // Command 초기화
            CommandLoad = new RelayCommand(Onload);


            // Timer 초기화
            _timer.Interval = TimeSpan.FromMinutes(10);
            _timer.Tick += async (s, e) => await GetWeatherInfo();
            _timer.Start();

            // HttpClientService 초기화
            _httpClientService = httpClientService;
        }

        public async Task GetWeatherInfo()
        {
            // OpenWeatherMap API 호출
            // https://api.openweathermap.org/data/2.5/weather?q=Seoul&appid=b043c91ecaf24a3abbcdeacddab91d7e

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={CityName}&appid={API_KEY}&units=metric";

            string response = string.Empty;
            try
            {
                response = await _httpClientService.GetAsync(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            JObject weatherData = JObject.Parse(response);
            Description = weatherData["weather"][0]["description"].ToString();
            Temperature = weatherData["main"]["temp"].ToString();
            Humidity = weatherData["main"]["humidity"].ToString();
            WeatherType = weatherData["weather"][0]["main"].ToString();
            Icon = $"/Resources/icon_{WeatherType}.png".ToLower();

            // Icon이 존재하지 않을 경우x 아이콘으로 대체
            var resource = new Uri(Icon, UriKind.Relative);
            try
            {
                var stream = Application.GetResourceStream(resource);
                if (stream != null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Icon = "/Resources/icon_x.png";
        }

        public void Dispose()
        {
            _timer.Stop();
        }
    }
}
