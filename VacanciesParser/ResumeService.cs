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
    
    /*foreach (var r in resumeList)
    {
      Console.WriteLine($"job name: {r.JobName}");
      Console.WriteLine($"salary: {r.Salary}\n");
    }*/

    Console.WriteLine($"всего резюме: {resumeList.Count}");

    for (int i = 0; i < resumeList.Count; i++)
    {
      resumeList[i].ProfessionalGroupName = SetProfessionalGroup(resumeList[i].JobName);
    }
    
    return resumeList;
  }

  private string SetProfessionalGroup(string jobName)
  {
    var name = this.professionalCategory.GetProfessionalGroupByJobName(jobName);
    
    return name;
  }
}