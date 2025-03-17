using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace MyWidgets.Common
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private Frame _frame;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo<T>() where T : Page
        {
            var page = _serviceProvider.GetRequiredService<T>();
            _frame.Navigate(page);
        }

        public void NavigateTo(Type pageType)
        {
            var page = (Page)_serviceProvider.GetRequiredService(pageType);
            _frame.Navigate(page);
        }
    }
}
