using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using YouTrack.Rest;

namespace YTConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      var username = ConfigurationSettings.AppSettings.Get("username");
      var password = ConfigurationSettings.AppSettings.Get("password");
      var ytURL = ConfigurationSettings.AppSettings.Get("ytURL");
      var project = ConfigurationSettings.AppSettings.Get("project");

      Console.WriteLine($"Account {username}");

      IYouTrackClient yt = new YouTrackClient(ytURL, username, password);
      var issues = yt.GetProjectRepository().GetProject(project).GetIssues("#Submitted #BUG -Resolved");

      var www = new WebClient();
      //fix basic auth
      var encoded =
        System.Convert.ToBase64String(
          System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
      www.Headers.Add("Authorization", "Basic " + encoded);


      Console.WriteLine($"size: {issues.Count()}");
      issues.ToList().ForEach(i =>
      {
        if (i.Description.Contains("The actual length is greater than the MaxLength value."))
        {
          Console.WriteLine("IS A LONG NAME BUG!");
          Console.WriteLine($"{i.Id} - {i.Summary} \n" +
                            $"{i.Description}");
          i.GetAttachments().ToList().ForEach(a =>
          {
            Console.WriteLine($"{a.Name}");
            if (a.Name.Contains(".json"))
            {
              using (www)
              {
                var rawdata = www.DownloadString(a.Url);
                Console.WriteLine(rawdata);
                var dataArr = JsonConvert.DeserializeObject<List<CommandJsonModel>>(rawdata);
                dataArr.ForEach(Console.WriteLine);
              }
            }
            else
            {
              using (www)
              {
                var rawdata = www.DownloadString(a.Url);
                Console.WriteLine(rawdata);
              }
            }
          });
        }
      });
    }
  }
}