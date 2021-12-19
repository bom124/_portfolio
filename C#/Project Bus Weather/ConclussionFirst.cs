using System;
using Newtonsoft.Json.Linq;

namespace ConsoleApp8
{
  class ConclussionFirst
  {
    public static void ConclussionFirstEnd(int count,JToken timetable)
    {
      for (int i = 0; i < count; i++)
      {
        Console.WriteLine(i + " Маршрут: " + timetable[i]["thread"]["short_title"]);
        Console.WriteLine("Время отправления: " + timetable[i]["departure"]);
        Console.WriteLine("Время прибытия: " + timetable[i]["arrival"]);
      }
    }
  }
}
