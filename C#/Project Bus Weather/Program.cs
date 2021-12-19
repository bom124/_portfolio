using System;
using Newtonsoft.Json.Linq;

namespace ConsoleApp8
{
  class Program
  {
    static void Main(string[] args)
    {
      while(true)
      {
        string month = Enter.EnterData("Введите месяц отправления");
        string data =Enter.EnterData("Введите дату отправления");
        string cityStart = Enter.EnterData("Введите город отправления: ");
        string cityEnd = Enter.EnterData("Введите город прибытия: ");
        JToken timetable = Timetable.TimetableGet(cityStart, cityEnd,month,data);
        if(timetable == null)
        {
          continue;
        }
        JToken pogodaCityStart = Weather.WeatherGet(cityStart);
        JToken pogodaCityEnd = Weather.WeatherGet(cityEnd);
        int count = ((JArray)timetable).Count;
        if(count==0)
        {
          Console.WriteLine("Маршрутов не обнаружено");
          continue;
        }
        ConclussionFirst.ConclussionFirstEnd(count, timetable);
        ConclusionEnd.ConclusionScreen(timetable, pogodaCityStart, pogodaCityEnd);
      }
      
    }
  }
}
