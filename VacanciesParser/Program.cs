using VacanciesParser;

class Program
{
  static async Task Main(string[] args)
  {
    ParserService parserService = new ParserService();
    
    string baseFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Downloads";
    
    string apiUrl = "http://opendata.trudvsem.ru/api/v1/vacancies/region/1800000000000";

    string vacanciesJSON = await GetVacanciesJson(apiUrl);

    List<VacancyWrapper> vacancies = DeserializeVacancies(vacanciesJSON);

    vacancies = await parserService.JoinVacancyViewAndResult(vacancies);
    
    parserService.WriteVacanciesToCsv(vacancies, baseFilePath, "vacancies");

    string vacanciesTablePath = $@"{baseFilePath}\vacancies.csv";
    
    string vacanciesStatisticTablePath = $@"{baseFilePath}\vacancies_statistic.csv";
    
    List<VacancyStatisticSample> vacancyStatisticSamples = parserService.ReadVacanciesCsv(vacanciesTablePath);

    List<VacancyStatistic> vacancyStatistics = parserService.CalculateVacancyStatistic(vacancyStatisticSamples);
    
    parserService.WriteSummaryTableToCsv(vacancyStatistics, vacanciesStatisticTablePath);
    
    WriteDiagrams(vacancyStatistics, baseFilePath);
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