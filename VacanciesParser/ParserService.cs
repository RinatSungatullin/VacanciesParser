using Microsoft.Playwright;

namespace VacanciesParser;

public class ParserService
{
  private FileService fileService;
  
  private Dictionary<string, List<string>> ProfessionalCategories = new Dictionary<string, List<string>>()
  {
    ["Руководители"] = new List<string>
    {
        "руководитель",
        "начальник",
        "заведующий",
        "ведущий",
        "директор",
        "заместитель"
    },

    ["Специалисты высшего уровня"] = new List<string>
    {
        "врач",
        "ветеринар",
        "инженер",
        "программист",
        "системный администратор",
        "юрисконсульт",
        "специалист",
        "инспектор",
        "лесничий",
        "муниципальный служащий",
        "юрис",
        "фармацевт",
        "оценщик",
        "энергетик",
        "проектировщик",
        "конструктор",
        "электроник",
        "бухгалтер"
    },

    ["Специалисты среднего уровня"] = new List<string>
    {
        "администратор",
        "продавец",
        "консультант",
        "педагог",
        "преподаватель",
        "воспитатель",
        "помощник воспитателя",
        "младший воспитатель",
        "инструктор",
        "тренер",
        "массажист",
        "медицинская сестра",
        "кондуктор",
        "токарь",
        "воспитат",
        "кондитер",
        "гример",
        "стрелок",
        "экскурсовод",
        "экспозицион",
        "кассир",
        "портье",
        "диспетчер",
        "товаровед",
        "оператор видеонаблюдения"
    },

    ["Рабочие промышленности, строительства, транспорта"] = new List<string>
    {
        "слесарь",
        "электромонтер",
        "электромеханик",
        "электрик",
        "техник",
        "повар",
        "пекарь",
        "рабочий",
        "работник",
        "мастер",
        "водитель",
        "грузчик",
        "кочегар",
        "приемщик",
        "паяльщик",
        "промывщик",
        "бурильщик",
        "правильщик",
        "полировщик",
        "сварщик",
        "электрогазосварщик",
        "фрезеровщик",
        "сверловщик",
        "монтажник",
        "маркировщик",
        "мойщик",
        "тракторист",
        "сборщик",
        "укладчик"
    },

    ["Операторы, аппаратчики, машинисты"] = new List<string>
    {
        "оператор",
        "машинист",
        "аппаратчик",
        "наладчик",
        "котельной",
        "машинного доения"
    },

    ["Неквалифицированные рабочие"] = new List<string>
    {
        "охранник",
        "дворник",
        "уборщик",
        "уборщица",
        "упаковщик",
        "укладчик-упаковщик"
    }
  };

  public ParserService()
  {
    this.fileService = new FileService();
  }
  
  public async Task<List<VacancyWrapper>> JoinVacancyViewAndResult(List<VacancyWrapper> vacancies)
  {
    Console.WriteLine("parsing html");
    HtmlParser parser = new HtmlParser();

    for (int i = 0; i < vacancies.Count; i++)
    {
      Console.WriteLine($"getting vacancy No {i + 1}");
      
      var currentVacancyUrl = vacancies[i].Vacancy.Url;

      Console.WriteLine($"vacancy id: {vacancies[i].Vacancy.Id}");
      
      string vacancyView = await parser.GetValueByKey(currentVacancyUrl, "Просмотры вакансии");

      if (vacancyView != null)
        vacancies[i].Vacancy.Views = vacancyView;
      
      Console.WriteLine($"views:{vacancies[i].Vacancy.Views}");
    }
    
    return vacancies;
  }

  public List<VacancyStatisticSample> ReadVacanciesCsv(string filePath, string fileName)
  {
    List<VacancyStatisticSample> vacancies = new List<VacancyStatisticSample>();
    
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

  private VacancyStatisticSample MapLinesToVacancyStatistics(string line)
  {
    Console.WriteLine("maping lines to vacancy statistics");
    
    string[] vacancySplit = line.Split(';');

    int averageSalary = (int.Parse(vacancySplit[2]) + int.Parse(vacancySplit[3])) / 2;

    
    int vacancyView = int.Parse(vacancySplit[7]);

    return new VacancyStatisticSample(GetProfessionalGroup(vacancySplit[1]), averageSalary, vacancyView);
  }

  private string GetProfessionalGroup(string vacancyName)
  {
    foreach (var p in  ProfessionalCategories)
    {
      foreach (var c in p.Value)
      {
        if (c == vacancyName.ToLower())
        {
          // Console.WriteLine(p.Key);
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
          // Console.WriteLine(p.Key);
          return p.Key;
        }
      }
    }
    
    return null;
  }

  public List<VacancyStatistic> CalculateVacancyStatistic(List<VacancyStatisticSample> vacancies)
  {
    var result = new List<VacancyStatistic>();

    foreach (var kvp in this.ProfessionalCategories)
    {
      string groupName = kvp.Key;

      var items = vacancies
        .Where(x => x.ProfessionalGroup == groupName)
        .ToList();

      int count = items.Count;

      int avgSalary = count > 0 ? (int)Math.Round(items.Average(x => x.AverageSalary)) : 0;

      int views = items.Sum(x => x.VacancyViews);

      result.Add(new VacancyStatistic(groupName, count, avgSalary, views));
    }

    return result;
  }

  public void WriteSummaryTableToCsv(List<VacancyStatistic>vacanciesStatistic, string vacancyStatisticFullPath)
  {
    VacancyStatisticCalculator statisticCalculator = new VacancyStatisticCalculator(
      vacanciesStatistic.Sum(x => x.Quantity),
      (int)vacanciesStatistic.Average(x => x.AverageSalary),
      vacanciesStatistic.Sum(x => x.Views)
      );
    
    this.fileService.WriteSummaryTableToCsv(vacanciesStatistic, statisticCalculator, vacancyStatisticFullPath);
  }
}