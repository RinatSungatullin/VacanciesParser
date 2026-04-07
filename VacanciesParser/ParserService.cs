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
      vacancies.Add(MapLinesToVacancyStatistics(line));
    }
    
    return vacancies;
  }

  private VacancyStatistic MapLinesToVacancyStatistics(string line)
  {
    string[] split = line.Split(';');

    int averageSalary = int.Parse(split[2]) + int.Parse(split[3]);
    
    int vacancyView = int.Parse(split[8]);

    return new VacancyStatistic("test group", averageSalary, vacancyView);
  }

  public string GetProfessionalGroup(string vacancyName)
  {
    
    
    return "";
  }
}