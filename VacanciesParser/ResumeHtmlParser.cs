using System.Text;
using Microsoft.Playwright;

namespace VacanciesParser;

public class ResumeHtmlParser
{
  public async Task<List<Resume>> ParseResume(List<Resume> resumeList, string htmlUrl)
  {
    //var resumeList = new List<Resume>();

    using var playwright = await Playwright.CreateAsync();

    var browser = await playwright.Chromium.LaunchAsync(new()
    {
      Headless = false
    });

    var page = await browser.NewPageAsync();

    await page.GotoAsync(htmlUrl);

    await page.WaitForSelectorAsync(".search-results-simple-card");

    for (int i = 1; i < 10; i++)
    {
      Console.WriteLine($"page: {i}");

      var button = page.Locator("button[data-action='append']");

      if (await button.CountAsync() == 0)
        break;

      await button.ScrollIntoViewIfNeededAsync();

      int before = (await page.QuerySelectorAllAsync(".search-results-simple-card")).Count;

      await button.ClickAsync();

      try
      {
        await page.WaitForFunctionAsync(
          $"document.querySelectorAll('.search-results-simple-card').length > {before}",
          null,
          new() { Timeout = 5000 }
        );
      }
      catch
      {
        Console.WriteLine("Новые данные не загрузились");
        break;
      }
    }

    var cards = await page.QuerySelectorAllAsync(".search-results-simple-card");

    foreach (var card in cards)
    {
      var nameEl = await card.QuerySelectorAsync("strong.search-results-simple-card__name");
      var salaryEl = await card.QuerySelectorAsync("div.search-results-simple-card__salary");

      var name = nameEl != null
        ? (await nameEl.InnerTextAsync()).Trim()
        : "нет названия";

      var salaryText = salaryEl != null
        ? await salaryEl.InnerTextAsync()
        : "";

      int salary = 0;

      if (!string.IsNullOrWhiteSpace(salaryText))
      {
        var digits = new string(salaryText.Where(char.IsDigit).ToArray());
        int.TryParse(digits, out salary);
      }

      resumeList.Add(new Resume
      {
        JobName = name,
        Salary = salary
      });
    }

    await browser.CloseAsync();

    return resumeList;
  }
}