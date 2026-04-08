using VacanciesParser;

class Program
{
  static async Task Main(string[] args)
  {
    /*VacancyApiClient apiClient = new VacancyApiClient();

    string url = "http://opendata.trudvsem.ru/api/v1/vacancies/region/1800000000000";

    var vacanciesJson = await apiClient.GetAllVacancies(url);
    
    JSONDeserializer deserializer = new JSONDeserializer();
    var deserializedVacancies = deserializer.DeserializeVacanciesJSON(vacanciesJson);
    
    ParserService parserService = new ParserService();
    
    deserializedVacancies = await parserService.JoinVacancyViewAndResult(deserializedVacancies);

    int i = 1;

    foreach (var v in deserializedVacancies)
    {
      Console.WriteLine($"vacancy no {i}");
      Console.WriteLine($"id: {v.Vacancy.Id}");
      Console.WriteLine($"name: {v.Vacancy.JobName}");
      Console.WriteLine($"salary from {v.Vacancy.SalaryMin}");
      Console.WriteLine($"salary to {v.Vacancy.SalaryMax}");
      Console.WriteLine($"vacancy url: {v.Vacancy.Url}");
      Console.WriteLine($"specialization: {v.Vacancy.Category.Specialisation}");
      Console.WriteLine($"education: {v.Vacancy.Requirement.Education}");
      Console.WriteLine($"responses: {v.Vacancy.Responses}");
      Console.WriteLine($"views: {v.Vacancy.Views}");
      Console.WriteLine("=============================================\n");

      i++;
    }*/

    // сохранение csv
     string vacanciesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Downloads";

    //Console.WriteLine(vacanciesFilePath);

    FileService fileService = new FileService();
    //fileService.WriteVacanciesToCsv(deserializedVacancies, vacanciesFilePath, "vacancies");

    // чтение vacancies.csv
    string vacanciesFullPath = $@"{vacanciesFilePath}\vacancies.csv";
    
    string vacancyStatisticFullPath = $@"{vacanciesFilePath}\vacancies_statistic.csv";

    ParserService ps = new ParserService();
    
    var vacancies = ps.ReadVacanciesCsv(vacanciesFullPath, "asd");

    var vacanciesStatistic = ps.CalculateVacancyStatistic(vacancies);

    foreach (var vc in vacanciesStatistic)
    {
      Console.WriteLine(vc.ProfessionalGroup);
      Console.WriteLine(vc.AverageSalary);
      Console.WriteLine(vc.Quantity);
      Console.WriteLine(vc.Views);
    }
    
    ps.WriteSummaryTableToCsv(vacanciesStatistic, vacancyStatisticFullPath);
  }
}