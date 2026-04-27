using System.Globalization;
using VacanciesParser;

class Program
{
  static async Task Main(string[] args)
  {
    // api удмуртия
    // string apiUrl = "http://opendata.trudvsem.ru/api/v1/vacancies/region/1800000000000";
    
    // api регион
    string regionKey = "izhevsk";
    string region = Regions[regionKey];
    string regionCode = RegionCodes[regionKey];
    
    string apiUrl = $"http://opendata.trudvsem.ru/api/v1/vacancies?text={region}";
    
    string vacancyHtmlUrl =
      "https://trudvsem.ru/vacancy/search?_regionIds=1800000000000&page=0&salary=0&salary=999999&scheduleType=FULL&vacancyType=LONG";
    
    string resumeUrl = $"https://trudvsem.ru/cv/search?_regionIds=1800000000000&_districts={regionCode}&page=0&salary=0&salary=999999&experience=EXP_STAFF&cvType=LONG";
    
    // директория пользователя
    // string baseFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/VacancyParser";
    
    string baseFilePath = $"{AppContext.BaseDirectory}/result";
    
    if (!Directory.Exists(baseFilePath))
      Directory.CreateDirectory($"{baseFilePath}");
    
    string vacanciesTablePath = $@"{baseFilePath}/vacancies_{region}.csv";
    
    string summaryStatisticTablePath = $@"{baseFilePath}/summary_statistic_{region}.csv";
    
    VacancyService vacancyService = new VacancyService();
    
    // получение вакансии json из api
    string vacanciesJSON = await GetVacanciesJson(apiUrl);

    List<VacancyWrapper> vacanciesWrapper = DeserializeVacancies(vacanciesJSON);

    Console.WriteLine($"total {vacanciesWrapper.Count} vacancies\n");
    
    // Получение просмотров вакансий из html
    vacanciesWrapper = await vacancyService.JoinVacancyView(vacanciesWrapper);
    
    List<Vacancy> vacancies = MapToVacancies(vacanciesWrapper);
    
    // Получение вакансий из html
    //List<Vacancy> vacancies = await vacancyService.GetVacancyFromPage(vacancyHtmlUrl);
    
    
    // запись вакансий в таблицу
    vacancyService.WriteVacanciesToCsv(vacancies, baseFilePath, $"vacancies_{region}");
    Console.WriteLine($"таблица вакансий сохранена: {vacanciesTablePath}");
    
    
    // чтение таблицы вакансий
    List<VacancyStatisticSample> vacancyStatisticSamples = vacancyService.ReadVacanciesCsv(vacanciesTablePath);

    List<VacancyStatistic> vacancyStatistics = vacancyService.CalculateVacancyStatistic(vacancyStatisticSamples);

    
    // запись диаграммы
    WriteDiagrams(vacancyStatistics, baseFilePath, region);
    Console.WriteLine($"диаграммы сохранены: {baseFilePath}");
   
 
    
    
    

    ResumeService resumeService = new ResumeService();

    var resumeList = await resumeService.GetResume(resumeUrl);

    var resumeStatistic = resumeService.GetResumeStatistic(resumeList);

    var summaryStatistics = GetSummaryStatistic(vacancyStatistics, resumeStatistic);

     // запись итоговой таблицы
     vacancyService.WriteSummaryTableToCsv(summaryStatistics, summaryStatisticTablePath);
     Console.WriteLine($"итоговая статистика сохранена: {summaryStatisticTablePath}");

     Console.WriteLine("Нажмите любую клавишу для завершения");
     
     Console.ReadKey();
  }

  private static Task<string> GetVacanciesJson(string url)
  {
    VacancyApiClient apiClient = new VacancyApiClient();
    
    return apiClient.GetAllVacancies(url);
  }

  private static List<VacancyWrapper> DeserializeVacancies(string vacanciesJSON)
  {
    JSONDeserializer deserializer = new JSONDeserializer();

    return deserializer.DeserializeVacanciesJSON(vacanciesJSON);
  }

  private static void WriteDiagrams(List<VacancyStatistic> vacancyStatistics, string filePath, string region)
  {
    DiagramService ds = new DiagramService();
    
    ds.WriteProfessionalGroupsCircleDiagram(vacancyStatistics, filePath, $"professionalGroups_{region}");
    ds.WriteSalaryLineDiagram(vacancyStatistics, filePath, $"VacanciesSalary_{region}");
  }

  private static List<SummaryStatistic> GetSummaryStatistic(List<VacancyStatistic> vacancies,
    List<ResumeStatistic> resumes)
  {
    var resumeData = resumes
      .GroupBy(x => x.ProfessionalGroupName)
      .ToDictionary(
        g => g.Key,
        g => new
        {
          Quantity = g.Sum(x => x.Quantity),
          AvgSalary = g.Any(x => x.AverageSalary > 0)
            ? (int)g.Average(x => x.AverageSalary)
            : 0
        });

    var vacancyData = vacancies
      .GroupBy(x => x.ProfessionalGroup)
      .ToDictionary(
        g => g.Key,
        g => new
        {
          Quantity = g.Sum(x => x.Quantity),
          AvgSalary = g.Any(x => x.AverageSalary > 0)
            ? (int)g.Average(x => x.AverageSalary)
            : 0,
          Views = g.Sum(x => x.Views)
        });

    var allKeys = resumeData.Keys
      .Union(vacancyData.Keys);

    return allKeys
      .Select(key =>
      {
        resumeData.TryGetValue(key, out var r);
        vacancyData.TryGetValue(key, out var v);

        return new SummaryStatistic(
          key,
          r?.Quantity ?? 0,
          r?.AvgSalary ?? 0,
          v?.Quantity ?? 0,
          v?.AvgSalary ?? 0,
          v?.Views ?? 0
        );
      })
      .ToList();
  }
  
  private static List<Vacancy> MapToVacancies(List<VacancyWrapper> wrappers)
  {
    return wrappers
      .Where(w => w.Vacancy != null)
      .Select(w => w.Vacancy)
      .ToList();
  }

  private static Dictionary<string, string> Regions = new Dictionary<string, string>()
  {
    ["izhevsk"] = "ижевск",
    ["votkinsk"] = "воткинск",
    ["sarapul"] = "сарапул",
    ["glazov"] = "глазов"
  };

  private static Dictionary<string, string> RegionCodes = new Dictionary<string, string>()
  {
    ["izhevsk"] = "1800000100000",
    ["votkinsk"] = "1800000300000",
    ["sarapul"] = "1800000200000",
    ["glazov"] = "1800000400000"
  };
}