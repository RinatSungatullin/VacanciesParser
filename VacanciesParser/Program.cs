using VacanciesParser;

class Program
{
  static async Task Main(string[] args)
  {
    VacancyApiClient apiClient = new VacancyApiClient();

    var vacanciesJson = await apiClient.GetAllVacancies("http://opendata.trudvsem.ru/api/v1/vacancies/region/1800000000000");
    
    JSONDeserializer deserializer = new JSONDeserializer();
    var deserialized = deserializer.DeserializeVacanciesJSON(vacanciesJson);

    /*HttpClient client =  new HttpClient();

    string url = "http://opendata.trudvsem.ru/api/v1/vacancies/region/1800000000000";

    string response = await client.GetStringAsync(url);

    // Console.WriteLine(response);
    var deserializedVacancies = JsonConvert.DeserializeObject<ApiResponseResult>(response);

    Console.WriteLine(deserializedVacancies.Results.Vacancies[0].Vacancy.JobName);*/

    string filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Downloads";

    Console.WriteLine(filePath);
    
    FileService fileService = new FileService();
    fileService.WriteVacanciesToCsv(deserialized, filePath, "vacancies");
    
    /*int i = 1;

    foreach (var v in deserialized)
    {
      Console.WriteLine($"vacancy no {i}");
      Console.WriteLine($"id: {v.Vacancy.Id}");
      Console.WriteLine($"name: {v.Vacancy.JobName}");
      Console.WriteLine($"salary from {v.Vacancy.SalaryMin}");
      Console.WriteLine($"salary to {v.Vacancy.SalaryMax}");
      Console.WriteLine($"vacancy url: {v.Vacancy.VacUrl}");
      Console.WriteLine($"specialization: {v.Vacancy.Category.Specialisation}");
      Console.WriteLine($"education: {v.Vacancy.Requirement.Education}");
      Console.WriteLine("=============================================\n");

      i++;
    }*/


  }
}