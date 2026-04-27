using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace VacanciesParser;

public class VacancyHtmlParser
{
  /// <summary>
  /// Поулчить просмотры вакансии
  /// </summary>
  /// <param name="htmlUrl">Ссылка на вакансию</param>
  /// <param name="keyValue">Блок просмотров из html</param>
  /// <returns>Значение просмотров</returns>
  /// <exception cref="Exception">Страница не доуступна</exception>
  public async Task<string> GetValueByKey(string htmlUrl, string keyValue)
  {
    /*Environment.SetEnvironmentVariable(
      "PLAYWRIGHT_BROWSERS_PATH",
      Path.Combine(AppContext.BaseDirectory, "ms-playwright")
    );*/
    
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
      Console.WriteLine($"try {attempt}: {htmlUrl}");

      var response = await page.GotoAsync(htmlUrl, new PageGotoOptions
      {
        WaitUntil = WaitUntilState.DOMContentLoaded,
        Timeout = 60000
      });

      Console.WriteLine($"status response: {response.Status}");

      if (response == null || response.Status != 200)
      {
        Console.WriteLine($"HTTP error: {response?.Status}");
        if (attempt == maxAttempts)
          throw new("vacancy page unavailable");

        await Task.Delay(3000);
        continue;
      }

      var bodyText = await page.InnerTextAsync("body");

      if (bodyText.Contains("Информация временно недоступна"))
      {
        Console.WriteLine("page is unavailable");
        if (attempt == maxAttempts)
          throw new Exception("information unavailable");

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
        throw new Exception("vacancy page unavailable");

      await Task.Delay(2000);
    }

    return null;
  }

  /// <summary>
  /// Получить вакансии из html по url.
  /// </summary>
  /// <param name="htmlUrl">Ссылка на вакансии.</param>
  /// <returns>Коллексция ссылок вакансий.</returns>
  public async Task<List<string>> GetVacancyLinks(string htmlUrl)
  {
    Environment.SetEnvironmentVariable(
      "PLAYWRIGHT_BROWSERS_PATH",
      Path.Combine(AppContext.BaseDirectory, "ms-playwright")
    );
    
    Console.WriteLine("getting vacancy urls");
    
    using var playwright = await Playwright.CreateAsync();

    var browser = await playwright.Chromium.LaunchAsync(new()
    {
      Headless = true
    });

    var page = await browser.NewPageAsync();

    await page.GotoAsync(htmlUrl);
    await page.WaitForSelectorAsync(".search-results-simple-card");

    for (int i = 0; i < 9; i++)
    {
      var button = page.Locator("button[data-action='append']");

      if (await button.CountAsync() == 0)
        break;

      int before = await page.Locator(".search-results-simple-card").CountAsync();

      await button.ScrollIntoViewIfNeededAsync();
      await button.ClickAsync();

      try
      {
        await page.WaitForFunctionAsync(
          $"document.querySelectorAll('.search-results-simple-card').length > {before}"
        );
      }
      catch
      {
        break;
      }
    }

    var cards = page.Locator(".search-results-simple-card");
    int count = await cards.CountAsync();

    if (count > 100)
      count = 100;

    var links = new List<string>(count);

    for (int i = 0; i < count; i++)
    {
      var card = cards.Nth(i);

      var link = card.Locator("a.search-results-simple-card__go-to-button");

      if (await link.CountAsync() == 0)
        continue;

      var href = await link.First.GetAttributeAsync("href");

      if (string.IsNullOrWhiteSpace(href))
        continue;

      string fullUrl = href.StartsWith("/")
        ? new Uri(new Uri(page.Url), href).ToString()
        : href;

      links.Add(fullUrl);
    }

    await browser.CloseAsync();

    Console.WriteLine($"received {links.Count} vacancies");

    return links;
  }

  /// <summary>
  /// Получить колекцию Vacancy из страницы по url.
  /// </summary>
  /// <param name="urls">Коллекция ссылок.</param>
  /// <returns>Коллекция Vacancy.</returns>
  public async Task<List<Vacancy>> GetVacancyListByUrl(List<string> urls)
  {
    Console.WriteLine("\nGetting vacancies by url");

    using var playwright = await Playwright.CreateAsync();

    var browser = await playwright.Chromium.LaunchAsync(new()
    {
        Headless = true
    });

    var page = await browser.NewPageAsync();

    // 🔥 уменьшаем таймауты (чтобы не ждать 30 сек)
    page.SetDefaultTimeout(10000);

    var result = new List<Vacancy>();

    int vacancyCounter = 1;

    foreach (var url in urls)
    {
        try
        {
            // -------------------------
            // ПЕРЕХОД (без NetworkIdle)
            // -------------------------
            await page.GotoAsync(url, new()
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 15000
            });

            // маленькая пауза → даём JS дорисовать DOM
            await page.WaitForTimeoutAsync(300);

            // -------------------------
            // ID
            // -------------------------
            var id = url.Split('/').Last();

            // -------------------------
            // JOB NAME
            // -------------------------
            string jobName = await SafeGetText(page.Locator("h1.content__title"));
            if (string.IsNullOrWhiteSpace(jobName))
                jobName = "не указано";

            // -------------------------
            // SALARY (без зависаний)
            // -------------------------
            int salaryMin = 0;
            int salaryMax = 0;

            var salaryText = await SafeGetText(page.Locator(".vacancy-sidebar__price"));

            salaryText = salaryText
                .Replace("\u00A0", " ")
                .Trim();

            var numbers = Regex.Matches(salaryText, @"\d[\d\s]*")
                .Select(m => int.Parse(m.Value.Replace(" ", "")))
                .ToList();

            if (numbers.Count > 0)
                salaryMin = numbers[0];

            if (numbers.Count > 1)
                salaryMax = numbers[1];

            // -------------------------
            // CATEGORY
            // -------------------------
            var category = await SafeGetText(
                page.Locator("dt:has-text('Сфера деятельности:') + dd")
            );

            if (string.IsNullOrWhiteSpace(category))
                category = "не указано";

            // -------------------------
            // EDUCATION
            // -------------------------
            var education = await SafeGetText(
                page.Locator("dt:has-text('Образование:') + dd")
            );

            if (string.IsNullOrWhiteSpace(education))
                education = "не указано";

            // -------------------------
            // VIEWS
            // -------------------------
            int views = 0;

            var viewsText = await SafeGetText(
                page.Locator("dd[data-content='views']")
            );

            int.TryParse(Regex.Match(viewsText, @"\d+").Value, out views);

            // -------------------------
            // RESULT
            // -------------------------
            var vacancy = new Vacancy
            {
                Id = id,
                JobName = jobName,
                SalaryMin = salaryMin,
                SalaryMax = salaryMax,
                Url = url,
                Views = views.ToString(),

                Category = new Category
                {
                    Specialisation = category
                },

                Requirement = new Requirement
                {
                    Education = education
                }
            };

            result.Add(vacancy);

            Console.WriteLine($"\nvacancy {vacancyCounter++}:");
            Console.WriteLine($"id: {vacancy.Id}");
            Console.WriteLine($"job name: {vacancy.JobName}");
            Console.WriteLine($"salary from: {vacancy.SalaryMin}");
            Console.WriteLine($"salary to: {vacancy.SalaryMax}");
            Console.WriteLine($"url: {vacancy.Url}");
            Console.WriteLine($"specialization: {vacancy.Category.Specialisation}");
            Console.WriteLine($"education: {vacancy.Requirement.Education}");
            Console.WriteLine($"vacancy views: {vacancy.Views}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR {url}: {ex.Message}");
        }
    }

    await browser.CloseAsync();

    return result;
  }
  
  private async Task<string> SafeGetText(ILocator locator)
  {
    try
    {
      if (await locator.CountAsync() == 0)
        return "";

      return await locator.EvaluateAsync<string>("el => el?.textContent ?? ''");
    }
    catch
    {
      return "";
    }
  }
}