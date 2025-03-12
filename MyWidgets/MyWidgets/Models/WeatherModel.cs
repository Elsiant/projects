using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWidgets.Models
{
    class WeatherModel
    {
        public string Icon { get; set; }
        public string CityName { get; set; }
        public string Description { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string WeatherType { get; set; }

        public WeatherModel() {
            
        }
    }
}
