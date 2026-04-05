using System.Net;
using System.Text.RegularExpressions;

namespace VacanciesParser;

public class HtmlParser
{
  public string GetVacancyViews(string htmlUrl)
  {
    return "";
  }

  public string GetVacancyResponses(string htmlUrl)
  {
    WebClient wc = new WebClient();
    
    string html = wc.DownloadString(htmlUrl);

    string[] splitHtml = html.Split('\n');
    
    int responsesIndex = Array.FindIndex(splitHtml, line => line.Contains("vacancyStatistics.countResponses"));
    
    Match match = Regex.Match(splitHtml[responsesIndex], @"default\((\d+)\)");

    return  match.Groups[1].Value;
  }
}