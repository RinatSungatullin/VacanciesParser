using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace VacanciesParser;

public class Requirement
{
  [JsonProperty("education")]
  public string Education { get; set; }
}