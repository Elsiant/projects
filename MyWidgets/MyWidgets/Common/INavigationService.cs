using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyWidgets.Common
{
    public interface INavigationService
    {
        void SetFrame(Frame frame);
        void NavigateTo<T>() where T : Page;
        void NavigateTo(Type pageType);
    }
}
