namespace VacanciesParser;

public class ResumeService
{
  private ProfessionalCategory professionalCategory;

  public ResumeService()
  {
    this.professionalCategory = new ProfessionalCategory();
  }
  
  public async Task<List<Resume>> GetResume(string htmlUrl)
  {
    ResumeHtmlParser  parser = new ResumeHtmlParser();

    List<Resume> resumeList = new List<Resume>();
    
    await parser.ParseResume(resumeList, htmlUrl);
    
    foreach (var r in resumeList)
    {
      Console.WriteLine($"job name: {r.JobName}");
      Console.WriteLine($"salary: {r.Salary}\n");
    }

    Console.WriteLine($"всего резюме: {resumeList.Count}");

    for (int i = 0; i < resumeList.Count; i++)
    {
      resumeList[i].ProfessionalGroupName = SetProfessionalGroup(resumeList[i].JobName);
    }
    
    return resumeList;
  }

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

  private string SetProfessionalGroup(string jobName)
  {
    var name = this.professionalCategory.GetProfessionalGroupByJobName(jobName);
    
    return name;
  }
}