using Newtonsoft.Json;

namespace VacanciesParser;

public class VacancyApiClient
{
  public async Task<string> GetAllVacancies(string url)
  {
    var handler = new HttpClientHandler()
    {
      Proxy = null,
      UseProxy = false
    };

    using var client = new HttpClient(handler);
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

    string response = await client.GetStringAsync(url);
    return response;
  }
}