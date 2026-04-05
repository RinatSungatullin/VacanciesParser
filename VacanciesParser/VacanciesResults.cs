using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace VacanciesParser;

public class VacanciesResults
{
  [JsonProperty("vacancies")]
  public List<VacancyWrapper> Vacancies { get; set; }
}