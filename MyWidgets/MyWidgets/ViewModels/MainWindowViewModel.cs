using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyWidgets.Common;
using MyWidgets.Commands;
using System.Windows.Controls;

namespace MyWidgets.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitCommand();
        }

        public ICommand NavigateCommand { get; set; }

        public override void InitCommand()
        {
            NavigateCommand = new RelayCommand<Type>(Navigate);
        }

        private void Navigate(Type pageType)
        {
            _navigationService.NavigateTo(pageType);
        }

        public void SetNavigationFrame(Frame frame)
        {
            _navigationService.SetFrame(frame);
            
        }
    }
}
