using Microsoft.Playwright;

namespace VacanciesParser;

public class ParserService
{
  private Dictionary<string, List<string>> ProfessionalCategories = new Dictionary<string, List<string>>()
  {
    ["Руководители"] = new List<string>
    {
      "руководитель",
      "начальник",
      "заведующий",
      "ведущий"
    },

    ["Специалисты высшего уровня"] = new List<string>
    {
      "врач",
      "инженер",
      "программист",
      "системный администратор",
      "юрисконсульт",
      "специалист",
      "инспектор",
      "лесничий",
      "муниципальный служащий"
    },

    ["Специалисты среднего уровня"] = new List<string>
    {
      "администратор",
      "продавец",
      "консультант",
      "педагог",
      "воспитатель",
      "помощник воспитателя",
      "младший воспитатель",
      "инструктор",
      "тренер",
      "фармацевт",
      "массажист",
      "медицинская сестра",
      "кондуктор"
    },

    ["Рабочие промышленности, строительства, транспорта"] = new List<string>
    {
      "слесарь",
      "электромонтер",
      "техник",
      "повар",
      "пекарь",
      "рабочий",
      "мастер",
      "водитель",
      "грузчик"
    },

    ["Операторы, аппаратчики, машинисты"] = new List<string>
    {
      "оператор",
      "машинист",
      "аппаратчик"
    },

    ["Неквалифицированные рабочие"] = new List<string>
    {
      "охранник"
    }
  };
    
  public async Task<List<VacancyWrapper>> JoinVacancyViewAndResult(List<VacancyWrapper> vacancies)
  {
    HtmlParser parser = new HtmlParser();

    for (int i = 0; i < vacancies.Count; i++)
    {
      var currentVacancyUrl = vacancies[i].Vacancy.Url;

      Console.WriteLine($"vacancy id: {vacancies[i].Vacancy.Id}");
      Console.WriteLine($"vacancy url: {currentVacancyUrl}");
      
      Console.WriteLine($"getting vacancy No {i + 1}");
      vacancies[i].Vacancy.Responses = await parser.GetValueByKey(currentVacancyUrl, "Количество откликов");
      Console.WriteLine($"responses result:{vacancies[i].Vacancy.Responses}");
      
      vacancies[i].Vacancy.Views = await parser.GetValueByKey(currentVacancyUrl, "Просмотры вакансии");
      Console.WriteLine($"views result:{vacancies[i].Vacancy.Views}");
    }
    
    return vacancies;
  }

  public List<VacancyStatistic> ReadVacanciesCsv(string filePath, string fileName)
  {
    List<VacancyStatistic> vacancies = new List<VacancyStatistic>();
    
    FileService fs = new FileService();

    string fullPath = $"{filePath}";
    
    List<string> lines = fs.ReadCsv(fullPath);

    foreach (var line in lines)
    {
      if (!string.IsNullOrWhiteSpace(line))
      {
        vacancies.Add(MapLinesToVacancyStatistics(line));
      }
    }
    
    return vacancies;
  }

  private VacancyStatistic MapLinesToVacancyStatistics(string line)
  {
    Console.WriteLine("maping lines to vacancy statistics");
    string[] vacancySplit = line.Split(';');

    int averageSalary = (int.Parse(vacancySplit[2]) + int.Parse(vacancySplit[3])) / 2;

    Console.WriteLine($"average salary: {averageSalary}");
    
    int vacancyView = int.Parse(vacancySplit[8]);

    Console.WriteLine($"vacancy view: {vacancyView}");

    return new VacancyStatistic(GetProfessionalGroup(vacancySplit[1]), averageSalary, vacancyView);
  }

  public string GetProfessionalGroup(string vacancyName)
  {
    foreach (var p in  ProfessionalCategories)
    {
      foreach (var c in p.Value)
      {
        if (c == vacancyName.ToLower())
        {
          Console.WriteLine(p.Key);
          return p.Key;
        }
      }
    }
    
    foreach (var p in  ProfessionalCategories)
    {
      foreach (var c in p.Value)
      {
        if (vacancyName.ToLower().Contains(c))
        {
          Console.WriteLine(p.Key);
          return p.Key;
        }
      }
    }
    
    return null;
  }
}