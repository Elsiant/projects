using System.Windows.Controls;
using MyWidgets.ViewModels;

namespace MyWidgets.Pages
{
    /// <summary>
    /// WeatherView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WeatherPage : Page
    {
        public WeatherPage(WeatherViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
