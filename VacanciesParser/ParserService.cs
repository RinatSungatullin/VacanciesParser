using Microsoft.Playwright;

namespace VacanciesParser;

public class ParserService
{
  public async Task<List<VacancyWrapper>> JoinVacancyViewAndResult(List<VacancyWrapper> vacancies)
  {
    HtmlParser parser = new HtmlParser();

    for (int i = 0; i < vacancies.Count; i++)
    {
      var currentVacancyUrl = vacancies[i].Vacancy.Url;

      Console.WriteLine($"vacancy id: {vacancies[i].Vacancy.Id}");
      Console.WriteLine($"vacancy url: {currentVacancyUrl}");
      
      Console.WriteLine($"getting responses vacancy No {i + 1}");
      vacancies[i].Vacancy.Responses = await parser.GetValueByKey(currentVacancyUrl, "Количество откликов");
      Console.WriteLine($"responses result:{vacancies[i].Vacancy.Responses}");
      
      Console.WriteLine($"getting views vacancy No {i + 1}");
      vacancies[i].Vacancy.Views = await parser.GetValueByKey(currentVacancyUrl, "Просмотры вакансии");
      Console.WriteLine($"views result:{vacancies[i].Vacancy.Views}");
    }
    
    return vacancies;
  }
}