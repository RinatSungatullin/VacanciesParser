using Newtonsoft.Json;

namespace VacanciesParser;

public class JSONDeserializer
{
  public List<VacancyWrapper> DeserializeVacanciesJSON(string vacanciesJSON)
  {
    var deserializedVacancies = JsonConvert.DeserializeObject<ApiResponseResult>(vacanciesJSON);

    return deserializedVacancies.Results.Vacancies;
  }
}