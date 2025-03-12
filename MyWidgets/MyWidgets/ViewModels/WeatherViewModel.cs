using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyWidgets.Commands;
using MyWidgets.Models;

namespace MyWidgets.ViewModels
{
    class WeatherViewModel : ViewModelBase
    {
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
            //임시 코드
            CityName = "Seoul";
            Description = "맑음";
            Temperature = "25";
            Humidity = "50";
            WeatherType = "맑음";
            Icon = "/Resources/icon_cloudy.png";
        }
        #endregion

        public WeatherViewModel()
        {
            CommandLoad = new RelayCommand(Onload);
        }
        public void OnUpdate()
        {
        }
    }
}
