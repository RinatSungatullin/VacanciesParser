namespace VacanciesParser;

public class ResumeService
{
  private ProfessionalCategory professionalCategory;

  public ResumeService()
  {
    this.professionalCategory = new ProfessionalCategory();
  }
  
  /// <summary>
  /// Получить резюме из html страницы.
  /// </summary>
  /// <param name="htmlUrl">Ссылка на резюме.</param>
  /// <returns>Список резюме.</returns>
  public async Task<List<Resume>> GetResume(string htmlUrl)
  {
    ResumeHtmlParser  parser = new ResumeHtmlParser();

    List<Resume> resumeList = new List<Resume>();
    
    await parser.ParseResume(resumeList, htmlUrl);

    Console.WriteLine($"total resume: {resumeList.Count}");

    Console.WriteLine("\ngetting groups:");
    for (int i = 0; i < resumeList.Count; i++)
    {
      resumeList[i].ProfessionalGroupName = SetProfessionalGroup(resumeList[i].JobName);

      Console.WriteLine($"\nvacancy: {resumeList[i].JobName}");
      Console.WriteLine($"group: {resumeList[i].ProfessionalGroupName}");
    }
    
    return resumeList;
  }

  /// <summary>
  /// Получить статистику по вакансиям.
  /// </summary>
  /// <param name="resumes">Список резюме.</param>
  /// <returns>Статистика по резюме.</returns>
  public List<ResumeStatistic> GetResumeStatistic(List<Resume> resumes)
  {
    return resumes
      .Where(x => !string.IsNullOrWhiteSpace(x.ProfessionalGroupName))
      .GroupBy(x => x.ProfessionalGroupName)
      .Select(g => new ResumeStatistic(
        g.Key,
        g.Count(),
        g.Any(x => x.Salary > 0)
          ? (int)g.Where(x => x.Salary > 0).Average(x => x.Salary)
          : 0
      ))
      .ToList();
  }

  /// <summary>
  /// Установить профессиональную группу для резюме.
  /// </summary>
  /// <param name="jobName">Название вакансии.</param>
  /// <returns>Группа.</returns>
  private string SetProfessionalGroup(string jobName)
  {
    var name = this.professionalCategory.GetProfessionalGroupByJobName(jobName);
    
    return name;
  }
}