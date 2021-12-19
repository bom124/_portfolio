using System;
using Newtonsoft.Json.Linq;

namespace ConsoleApp8
{
  class Timetable
  {
    public static JToken TimetableGet(string cityStart,string cityEnd,string month,string data)
    {
      int cityCodeStart = Base.CityId(cityStart);
      int cityCodeEnd = Base.CityId(cityEnd);
      int MonthNumber = Base.NumberMonth(month);
      string url = "https://api.rasp.yandex.net/v3.0/search/?apikey=bf79b804-f88a-4a3d-8f28-4814ece0a204&format=json&from=c"+cityCodeStart+"&to=c"+cityCodeEnd+"&transport_types=bus&lang=ru_RU&page=1&date=2021-"+ MonthNumber + "-"+ data;
      string responce = null;
      try
      {
        responce = NetConnect.respoce(url);
      }
      catch
      {
        Console.WriteLine("Маршрутов не обнаружено");
        return null;
      }
      JObject jObject = JObject.Parse(responce);
      return jObject["segments"];
    }
  }
}
