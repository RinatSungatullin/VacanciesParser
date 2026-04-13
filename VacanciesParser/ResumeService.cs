namespace VacanciesParser;

public class ResumeService
{
  public async Task GetResume(string html)
  {
    ResumeHtmlParser  parser = new ResumeHtmlParser();
    
    var resumeList = await parser.ParseResume(html);

    foreach (var r in resumeList)
    {
      Console.WriteLine($"job name: {r.JobName}");
      Console.WriteLine($"salary: {r.Salary}\n");
    }

    Console.WriteLine($"всего резюме: {resumeList.Count}");
  }
}