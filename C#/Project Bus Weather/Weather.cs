
using Newtonsoft.Json.Linq;

namespace ConsoleApp8
{
  class Weather
  {

    static public JToken WeatherGet(string cityName)
    {
      string url = "https://api.openweathermap.org/data/2.5/weather?q="+cityName+"&lang=ru&units=metric&appid=05e715f0e69938db0670c548881d1900";
      string responce = NetConnect.respoce(url);
      JObject jObject = JObject.Parse(responce);
      return jObject;
    }
  }
}
