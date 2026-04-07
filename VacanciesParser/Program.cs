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
    string filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Downloads";

    /*Console.WriteLine(filePath);

    FileService fileService = new FileService();
    fileService.WriteVacanciesToCsv(deserializedVacancies, filePath, "vacancies");*/
    
    ParserService ps = new ParserService();
    ps.ReadVacanciesCsv($"{filePath}/vacancies.csv", "asd");
  }
}