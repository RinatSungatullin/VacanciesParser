using Microsoft.Playwright;

namespace VacanciesParser;

public class VacancyService
{
  private FileService fileService;

  private ProfessionalCategory professionalCategory;

  public VacancyService()
  {
    this.fileService = new FileService();
    
    this.professionalCategory = new ProfessionalCategory();
  }
  
  public async Task<List<VacancyWrapper>> JoinVacancyView(List<VacancyWrapper> vacancies)
  {
    Console.WriteLine("parsing html");
    VacancyHtmlParser parser = new VacancyHtmlParser();

    for (int i = 0; i < vacancies.Count; i++)
    {
      Console.WriteLine($"getting vacancy No {i + 1}");
      
      var currentVacancyUrl = vacancies[i].Vacancy.Url;

      Console.WriteLine($"vacancy id: {vacancies[i].Vacancy.Id}");

      try
      {
        string vacancyView = await parser.GetValueByKey(currentVacancyUrl, "Просмотры вакансии");

        if (!string.IsNullOrEmpty(vacancyView))
        {
          vacancies[i].Vacancy.Views = vacancyView;
        }
      
        Console.WriteLine($"views:{vacancies[i].Vacancy.Views}");
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        
        vacancies.RemoveAt(i);

        i--;
      }
    }
    
    return vacancies;
  }

  public List<VacancyStatisticSample> ReadVacanciesCsv(string filePath)
  {
    List<VacancyStatisticSample> vacancies = new List<VacancyStatisticSample>();
    
    FileService fs = new FileService();

    
    List<string> lines = fs.ReadCsv(filePath);

    foreach (var line in lines)
    {
      if (!string.IsNullOrWhiteSpace(line))
      {
        vacancies.Add(MapLinesToVacancyStatistics(line));
      }
    }
    
    return vacancies;
  }

  private VacancyStatisticSample MapLinesToVacancyStatistics(string line)
  {
    string[] vacancySplit = line.Split(';');

    int averageSalary = 0;
    
    if (int.Parse(vacancySplit[3]) == 0)
    {
      averageSalary = int.Parse(vacancySplit[2]);
    }
    else
    {
      averageSalary = (int.Parse(vacancySplit[2]) + int.Parse(vacancySplit[3])) / 2;
    }
    
    int vacancyView = int.Parse(vacancySplit[7]);

    return new VacancyStatisticSample(this.professionalCategory.GetProfessionalGroupByJobName(vacancySplit[1]), averageSalary, vacancyView);
  }

  public void WriteVacanciesToCsv(List<VacancyWrapper> vacancies, string vacanciesFilePath, string fileName)
  {
    this.fileService.WriteVacanciesToCsv(vacancies, vacanciesFilePath, fileName);
  }

  public List<VacancyStatistic> CalculateVacancyStatistic(List<VacancyStatisticSample> vacancies)
  {
    var result = new List<VacancyStatistic>();

    foreach (var kvp in this.professionalCategory.GetProfessionalCategories())
    {
      string groupName = kvp.Key;

      var items = vacancies
        .Where(x => x.ProfessionalGroup == groupName)
        .ToList();

      int count = items.Count;

      int avgSalary = count > 0 ? (int)Math.Round(items.Average(x => x.AverageSalary)) : 0;

      int views = items.Sum(x => x.VacancyViews);

      result.Add(new VacancyStatistic(groupName, count, avgSalary, views));
    }

    return result;
  }

  public void WriteSummaryTableToCsv(List<VacancyStatistic>vacanciesStatistic,
                                      string vacancyStatisticFullPath)
  {
    VacancyStatisticCalculator statisticCalculator = new VacancyStatisticCalculator(
      vacanciesStatistic.Sum(x => x.Quantity),
      (int)vacanciesStatistic.Average(x => x.AverageSalary),
      vacanciesStatistic.Sum(x => x.Views)
      );
    
    this.fileService.WriteSummaryTableToCsv(vacanciesStatistic, statisticCalculator, vacancyStatisticFullPath);
  }

  public List<VacancyWrapper> FixEmptyValue(List<VacancyWrapper> vacancies)
  {
    for (int i = 0; i < vacancies.Count; i++)
    {
      if (string.IsNullOrEmpty(vacancies[i].Vacancy.Views))
      {
        vacancies[i].Vacancy.Views = "0";
      }
    }
    
    return vacancies;
  }
}