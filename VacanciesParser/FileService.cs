using System.Text;

namespace VacanciesParser;

public class FileService
{
  public void WriteVacanciesToCsv(List<Vacancy> vacancies, string filePath, string fileName)
  {
    string fullPath = Path.Combine(filePath, $"{fileName}.csv");

    using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8))
    {
      sw.WriteLine("vacancy_id;vacancy_name;salary_from;salary_to;vacancy_url;" +
                   "specialization;education;vacancy_views;");

      foreach (var v in vacancies)
      {
        sw.WriteLine($"{v.Id};{v.JobName};{v.SalaryMin};{v.SalaryMax};" +
                     $"{v.Url};{v.Category.Specialisation};{v.Requirement.Education};" +
                     $"{v.Views}");
      }
    }
  }

  public List<string> ReadCsv(string filePath)
  {
    List<string> lines = new List<string>();

    
    using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
    {
      string line = sr.ReadLine();

      while (line != null)
      {
        line = sr.ReadLine();
        
        lines.Add(line);
      }
    }

    return lines;
  }

  public void WriteSummaryTableToCsv(List<SummaryStatistic> statistic, SummaryStatisticCalculator statisticCalculator, string fullPath)
  {
    using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8))
    {
      sw.WriteLine("Профессиональная группа;Количество заявленных вакансий;Средняя заработная плата;количество просмотров соискателями;" +
                   "Количество размещенных резюме;Средняя желаемая заработная плата;Напряженность на рынке труда");
      foreach (var v in statistic)
      {
        sw.WriteLine($"{v.ProfessionalGroup};{v.VacancyQuantity};{v.VacancyAverageSalary};{v.VacancyViews};" +
                     $"{v.ResumeQuantity};{v.ResumeAverageSalary};{v.Intensity}");
      }
      
      sw.WriteLine($"Итог;{statisticCalculator.TotalVacancies};{statisticCalculator.TotalSalaryAverage};{statisticCalculator.TotalViews};" +
                   $"{statisticCalculator.TotalResume};{statisticCalculator.AverageSalaryResume};{statisticCalculator.AverageIntensity}");
    }
  }
}