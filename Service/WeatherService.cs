using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json;
using Weather.WebApi.Models;

namespace Weather.WebApi.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly string apikey = "4677f68651d2c8ba0d0d4fdd2b394ded";
        private readonly GlobalWeather.GlobalWeatherSoapClient _weatherclient;
        public WeatherService(GlobalWeather.GlobalWeatherSoapClient weatherclient)
        {
            _weatherclient = weatherclient;
        }

        public List<string> RetrieveCities(string country)
        {
            try
            {
                //return null;
                GlobalWeather.GlobalWeatherSoapClient obj = new GlobalWeather.GlobalWeatherSoapClient();
                var response = obj.GetCitiesByCountry(country);

                var xmlDoc = XDocument.Parse(response);
                IEnumerable<string> result = from node in xmlDoc.Root.Descendants("City")
                                             select node.Value;

                return result.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<WeatherModel> GetWeather(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={apikey}&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
                    return new WeatherModel
                    {
                        Location = rawWeather.Name,
                        Temperature = rawWeather.Main.Temp + " Degree Centigrade",
                        Time = DateTime.Now.ToLocalTime().ToString(),
                        Visibility = rawWeather.Visibility + " miles per hour",
                        Summary = string.Join(",", rawWeather.Weather.Select(x => x.Main))
                    };
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
    public class OpenWeatherResponse
    {
        public string Name { get; set; }

        public IEnumerable<WeatherDescription> Weather { get; set; }

        public Main Main { get; set; }
        public Int32 Visibility { get; set; }

    }

    public class WeatherDescription
    {
        public string Main { get; set; }
        public string Description { get; set; }
    }

    public class Main
    {
        public string Temp { get; set; }
    }

    public interface IWeatherService
    {
        List<string> RetrieveCities(string country);
        Task<WeatherModel> GetWeather(string city);
    }
}