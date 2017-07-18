using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.WebApi.Models
{
    public class WeatherModel
    {
        public string Location { get; set; }
        public string Time { get; set; }
        public string Visibility { get; set; }
        public string Summary { get; set; }
        public string Temperature { get; set; }
        
    }
}