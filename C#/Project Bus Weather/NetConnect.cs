using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp8
{
  class NetConnect
  {
    public static string respoce(string url)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
      HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
      using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
      {
        return streamReader.ReadToEnd();

      }

    }
  }
}
