using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Weather.WebApi.Models;
using Weather.WebApi.Service;

namespace Weather.WebApi.Controllers
{
    [RoutePrefix("api/Weather")]
    public class WeatherController : ApiController
    {        
        public readonly IWeatherService _weatherService;
        
        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
                
        [Route("GetCities")]
        public List<string> Get(string country)
        {
            return _weatherService.RetrieveCities(country);
            
        }

        [Route("RetrieveWeather")]
        [HttpGet]
        public async Task<WeatherModel> GetWeather(string city="london")
        {
            return await _weatherService.GetWeather(city);

        }
    }
    
}
