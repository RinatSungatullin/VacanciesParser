using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace VacanciesParser;

public class ApiResponseResult
{
  [JsonProperty("results")]
  public VacanciesResults Results { get; set; }
}