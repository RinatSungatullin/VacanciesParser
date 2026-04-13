using VacanciesParser;

class Program
{
  static async Task Main(string[] args)
  {
    /*ParserService parserService = new ParserService();
    
    string baseFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Downloads";
    
    string apiUrl = "http://opendata.trudvsem.ru/api/v1/vacancies/region/1800000000000";

    // получение вакансии json из api
    string vacanciesJSON = await GetVacanciesJson(apiUrl);

    List<VacancyWrapper> vacancies = DeserializeVacancies(vacanciesJSON);

    // Получение просмотров вакансий из html
    vacancies = await parserService.JoinVacancyViewAndResult(vacancies);
    
    // запись вакансий в таблицу
    parserService.WriteVacanciesToCsv(vacancies, baseFilePath, "vacancies");

    string vacanciesTablePath = $@"{baseFilePath}\vacancies.csv";
    
    string vacanciesStatisticTablePath = $@"{baseFilePath}\vacancies_statistic.csv";
    
    // чтение таблицы вакансий
    List<VacancyStatisticSample> vacancyStatisticSamples = parserService.ReadVacanciesCsv(vacanciesTablePath);

    List<VacancyStatistic> vacancyStatistics = parserService.CalculateVacancyStatistic(vacancyStatisticSamples);
    
    // запись итоговой таблицы
    parserService.WriteSummaryTableToCsv(vacancyStatistics, vacanciesStatisticTablePath);
    
    // запись диаграммы
    WriteDiagrams(vacancyStatistics, baseFilePath);*/

    string resumeUrl = "https://trudvsem.ru/cv/search?_regionIds=1800000000000&page=0&salary=0&salary=999999&experience=EXP_STAFF&cvType=LONG";

    ResumeService resumeService = new ResumeService();
    
    await resumeService.GetResume(resumeUrl);
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