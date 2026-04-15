using VacanciesParser;

class Program
{
  static async Task Main(string[] args)
  {
    VacancyService vacancyService = new VacancyService();
    
    string baseFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Downloads";
    
    string apiUrl = "http://opendata.trudvsem.ru/api/v1/vacancies/region/1800000000000";

    // получение вакансии json из api
    string vacanciesJSON = await GetVacanciesJson(apiUrl);

    List<VacancyWrapper> vacancies = DeserializeVacancies(vacanciesJSON);

    // Получение просмотров вакансий из html
    vacancies = await vacancyService.JoinVacancyView(vacancies);
    
    // запись вакансий в таблицу
    vacancyService.WriteVacanciesToCsv(vacancies, baseFilePath, "vacancies");

    string vacanciesTablePath = $@"{baseFilePath}/vacancies.csv";
    
    string vacanciesStatisticTablePath = $@"{baseFilePath}/vacancies_statistic.csv";
    
    // чтение таблицы вакансий
    List<VacancyStatisticSample> vacancyStatisticSamples = vacancyService.ReadVacanciesCsv(vacanciesTablePath);

    List<VacancyStatistic> vacancyStatistics = vacancyService.CalculateVacancyStatistic(vacancyStatisticSamples);
    
    // запись диаграммы
    WriteDiagrams(vacancyStatistics, baseFilePath);

    string resumeUrl = "https://trudvsem.ru/cv/search?_regionIds=1800000000000&page=0&salary=0&salary=999999&experience=EXP_STAFF&cvType=LONG";

    ResumeService resumeService = new ResumeService();
    
    var resumeList = await resumeService.GetResume(resumeUrl);

    var resumeStatistic = resumeService.GetResumeStatistic(resumeList);

    var summaryStatistics = GetSummaryStatistic(vacancyStatistics, resumeStatistic);

     // запись итоговой таблицы
        vacancyService.WriteSummaryTableToCsv(summaryStatistics, vacanciesStatisticTablePath);
    
    foreach (var s in summaryStatistics)
    {
      Console.WriteLine($"professional group: {s.ProfessionalGroup}");
      
      Console.WriteLine($"vacancy quantity: {s.VacancyQuantity}");
      
      Console.WriteLine($"vacancy salary: {s.VacancyAverageSalary}");
      Console.WriteLine($"vacancy views: {s.VacancyViews}");
      Console.WriteLine($"resume quantity: {s.ResumeQuantity}");
      Console.WriteLine($"resume salary: {s.ResumeAverageSalary}");

      Console.WriteLine();
    }
    
    /*foreach (var r in resumeStatistic)
    {
      Console.WriteLine($"professional group: {r.ProfessionalGroupName}");
      Console.WriteLine($"vacancies quantity: {r.Quantity}");
      Console.WriteLine($"average salary: {r.AverageSalary}");
    }*/
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

  private static void WriteDiagrams(List<VacancyStatistic> vacancyStatistics, string filePath)
  {
    DiagramService ds = new DiagramService();
    
    ds.WriteVacanciesCircleDiagram(vacancyStatistics, filePath);
    ds.WriteSalaryLineDiagram(vacancyStatistics, filePath);
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
}