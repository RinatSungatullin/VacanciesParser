using System.Globalization;
using System.Text;
using VacanciesParser;

class Program
{
  static async Task Main(string[] args)
  {
    /*VacancyApiClient apiClient = new VacancyApiClient();

    string url = "http://opendata.trudvsem.ru/api/v1/vacancies/region/1800000000000";

    var vacanciesJson = await apiClient.GetAllVacancies(url);
    
    JSONDeserializer deserializer = new JSONDeserializer();
    var deserialized = deserializer.DeserializeVacanciesJSON(vacanciesJson);*/

    string htmlUrl = "https://trudvsem.ru/vacancy/card/1027700404797/dd5e1240-3076-11f1-beff-fde346cfb777";
    
    HtmlParser parser = new HtmlParser();
    
    string vacancyResponses = parser.GetVacancyResponses(htmlUrl);

    Console.WriteLine(vacancyResponses);


    /*int i = 1;

    foreach (var v in deserialized)
    {
      Console.WriteLine($"vacancy no {i}");
      Console.WriteLine($"id: {v.Vacancy.Id}");
      Console.WriteLine($"name: {v.Vacancy.JobName}");
      Console.WriteLine($"salary from {v.Vacancy.SalaryMin}");
      Console.WriteLine($"salary to {v.Vacancy.SalaryMax}");
      Console.WriteLine($"vacancy url: {v.Vacancy.VacancyUrl}");
      Console.WriteLine($"specialization: {v.Vacancy.Category.Specialisation}");
      Console.WriteLine($"education: {v.Vacancy.Requirement.Education}");
      Console.WriteLine("=============================================\n");

      i++;
    }*/

    // сохранение csv
    /*string filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Downloads";

    Console.WriteLine(filePath);

    FileService fileService = new FileService();
    fileService.WriteVacanciesToCsv(deserialized, filePath, "vacancies");*/
  }
}