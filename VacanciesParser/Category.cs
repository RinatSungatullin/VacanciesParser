using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace VacanciesParser;

public class Category
{
  [JsonProperty("specialisation")]
  public string Specialisation { get; set; }
}