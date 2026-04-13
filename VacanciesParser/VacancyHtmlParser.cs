using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace VacanciesParser;

public class VacancyHtmlParser
{
  public async Task<string> GetValueByKey(string htmlUrl, string keyValue)
  {
    using var playwright = await Playwright.CreateAsync();

    var browser = await playwright.Chromium.LaunchAsync(new()
    {
      Headless = true
    });

    var context = await browser.NewContextAsync(new()
    {
      UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
      Locale = "ru-RU"
    });

    var page = await context.NewPageAsync();

    int maxAttempts = 10;

    for (int attempt = 1; attempt <= maxAttempts; attempt++)
    {
      try
      {
        Console.WriteLine($"try {attempt}: {htmlUrl}");

        var response = await page.GotoAsync(htmlUrl, new PageGotoOptions
        {
          WaitUntil = WaitUntilState.DOMContentLoaded,
          Timeout = 60000
        });

        if (response == null || response.Status != 200)
        {
          Console.WriteLine($"HTTP error: {response?.Status}");
          if (attempt == maxAttempts)
            return null;

          await Task.Delay(3000);
          continue;
        }

        var bodyText = await page.InnerTextAsync("body");

        if (bodyText.Contains("Информация временно недоступна"))
        {
          Console.WriteLine("page is unavalable");
          if (attempt == maxAttempts)
            return null;

          await Task.Delay(3000);
          continue;
        }

        await page.WaitForFunctionAsync(
          @"(text) => document.body.innerText.includes(text)",
          keyValue,
          new PageWaitForFunctionOptions { Timeout = 30000 }
        );

        string content = await page.ContentAsync();
        var match = Regex.Match(content,
          $@"<dt[^>]*>\s*{Regex.Escape(keyValue)}\s*</dt>\s*<dd[^>]*>\s*(\d+)\s*</dd>",
          RegexOptions.IgnoreCase | RegexOptions.Singleline);

        if (match.Success)
          return match.Groups[1].Value;

        Console.WriteLine("value not found");
        if (attempt == maxAttempts)
          return null;

        await Task.Delay(2000);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"error on {attempt}: {ex.Message}");

        if (attempt == maxAttempts)
          return null;

        await Task.Delay(3000);
      }
    }

    return null;
  }
}