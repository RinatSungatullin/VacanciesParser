namespace VacanciesParser;

public class ResumeService
{
  private ProfessionalCategory professionalCategory;

  public ResumeService()
  {
    this.professionalCategory = new ProfessionalCategory();
  }
  
  public async Task GetResume(string htmlUrl)
  {
    ResumeHtmlParser  parser = new ResumeHtmlParser();

    List<Resume> resumeList = new List<Resume>();
    
    await parser.ParseResume(resumeList, htmlUrl);
    
    // var resumeList = await parser.ParseResume(html);

    foreach (var r in resumeList)
    {
      Console.WriteLine($"job name: {r.JobName}");
      Console.WriteLine($"salary: {r.Salary}\n");
    }

    Console.WriteLine($"всего резюме: {resumeList.Count}");
  }

  private string SetProfessionalGroup(string jobName)
  {
    
    
    return "";
  }
}