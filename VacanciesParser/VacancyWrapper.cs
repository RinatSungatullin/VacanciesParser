using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace VacanciesParser;

public class VacancyWrapper
{
  [JsonProperty("vacancy")]
  public Vacancy Vacancy { get; set; }
}