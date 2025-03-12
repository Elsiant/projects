using System.Windows.Controls;
using MyWidgets.ViewModels;

namespace MyWidgets.Widgets
{
    /// <summary>
    /// WeatherView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WeatherView : UserControl
    {
        private WeatherViewModel _viewModels = new WeatherViewModel();
        public WeatherView()
        {
            InitializeComponent();
            DataContext = _viewModels;
        }
    }
}
