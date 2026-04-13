using VacanciesParser;

class Program
{
  static async Task Main(string[] args)
  {
    /*VacancyService vacancyService = new VacancyService();
    
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
    
    // запись итоговой таблицы
    vacancyService.WriteSummaryTableToCsv(vacancyStatistics, vacanciesStatisticTablePath);
    
    // запись диаграммы
    WriteDiagrams(vacancyStatistics, baseFilePath);*/

    /*string resumeUrl = "https://trudvsem.ru/cv/search?_regionIds=1800000000000&page=0&salary=0&salary=999999&experience=EXP_STAFF&cvType=LONG";

    ResumeService resumeService = new ResumeService();
    
    var resumeList = await resumeService.GetResume(resumeUrl);

    foreach (var r in resumeList)
    {
      Console.WriteLine($"job name: {r.JobName}");
      
      Console.WriteLine($"group: {r.ProfessionalGroupName}");
      
      Console.WriteLine($"salary: {r.Salary}\n");
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
}