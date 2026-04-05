using Newtonsoft.Json;

namespace VacanciesParser;

public class VacancyApiClient
{
  public async Task<string> GetAllVacancies(string url)
  {
    HttpClient client =  new HttpClient();
    
    string response = await client.GetStringAsync(url);

    return response;
  }
}