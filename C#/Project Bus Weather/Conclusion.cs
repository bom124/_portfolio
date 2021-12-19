using System;
using Newtonsoft.Json.Linq;

namespace ConsoleApp8
{
  class ConclusionEnd
  {
    public static void ConclusionScreen( JToken dyv,JToken pogodaCityStart,JToken pogodaCityEnd)
    {
      int vubor = int.Parse(Console.ReadLine());
      try
      {
        Console.WriteLine("Маршрут: " + dyv[vubor]["thread"]["title"]);
        Console.WriteLine("Точное место отправления: " + dyv[vubor]["from"]["title"]);
        Console.WriteLine("Погода в городе отправления: " + pogodaCityStart["main"]["temp"] + "°C " + pogodaCityStart["weather"][0]["description"]);
        Console.WriteLine("Время отправления: " + dyv[vubor]["departure"]);
        Console.WriteLine("Точное место прибытия: " + dyv[vubor]["to"]["title"]);
        Console.WriteLine("Время прибытия: " + dyv[vubor]["arrival"]);
        Console.WriteLine("Погода в городе прибытия: " + pogodaCityEnd["main"]["temp"] + "°C " + pogodaCityEnd["weather"][0]["description"]);
      }
      catch
      {
        Console.WriteLine("Данные о маршруте не обнаружены");
      }
      
    }
  }
}
