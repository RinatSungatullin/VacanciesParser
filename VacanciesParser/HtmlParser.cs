using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace VacanciesParser;

public class HtmlParser
{
  public async Task<string> GetValueByKey(string htmlUrl, string keyValue)
  {
    using var playwright = await Playwright.CreateAsync();

    var browser = await playwright.Chromium.LaunchAsync(new()
    {
      // Headless = false
      Headless = true
    });

    var context = await browser.NewContextAsync(new()
    {
      UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
      Locale = "ru-RU"
    });

    var page = await context.NewPageAsync();

    try
    {
      var response = await page.GotoAsync(htmlUrl, new PageGotoOptions
      {
        WaitUntil = WaitUntilState.DOMContentLoaded,
        Timeout = 60000
      });

      if (response == null || response.Status != 200)
      {
        Console.WriteLine($"HTTP ошибка: {response?.Status}");
        return "blocked";
      }

      await page.WaitForFunctionAsync(
        $"() => document.body.innerText.includes('{keyValue}')",
        new PageWaitForFunctionOptions
        {
          Timeout = 60000
        }
      );

      string content = await page.ContentAsync();

      var match = Regex.Match(content,
        $@"<dt[^>]*>\s*{Regex.Escape(keyValue)}\s*</dt>\s*<dd[^>]*>\s*(\d+)\s*</dd>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline);

      return match.Success ? match.Groups[1].Value : "not found";
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Ошибка: {ex.Message}");
      return "error";
    }
  }
}