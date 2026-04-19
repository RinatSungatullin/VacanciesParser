using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace VacanciesParser;

public class Vacancy
{
  [JsonProperty("id")]
  public string Id { get; set; }
  
  [JsonProperty("job-name")]
  public string JobName { get; set; }
  
  [JsonProperty("salary_min")]
  public int SalaryMin { get; set; }
  
  [JsonProperty("salary_max")]
  public int SalaryMax { get; set; }
  
  [JsonProperty("vac_url")]
  public string Url { get; set; }
  
  [JsonProperty("category")]
  public Category Category { get; set; }
  
  [JsonProperty("requirement")]
  public Requirement Requirement { get; set; }
  
  public string Views { get; set; }
}