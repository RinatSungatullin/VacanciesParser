using System.Text;

namespace VacanciesParser;

public class FileService
{
  public void WriteVacanciesToCsv(List<VacancyWrapper> vacancies, string filePath, string fileName)
  {
    string fullPath = Path.Combine(filePath, $"{fileName}.csv");

    using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8))
    {
      sw.WriteLine("vacancy_id;vacancy_name;salary_from;salary_to;vacancy_url;" +
                   "specialization;education;vacancy_views;");

      foreach (var v in vacancies)
      {
        sw.WriteLine($"{v.Vacancy.Id};{v.Vacancy.JobName};{v.Vacancy.SalaryMin};{v.Vacancy.SalaryMax};" +
                     $"{v.Vacancy.Url};{v.Vacancy.Category.Specialisation};{v.Vacancy.Requirement.Education};" +
                     $"{v.Vacancy.Views}");
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

  public void WriteSummaryTableToCsv(List<SummaryStatistic> statistic, VacancyStatisticCalculator statisticCalculator, string fullPath)
  {
    using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8))
    {
      sw.WriteLine("Профессиональная группа;Количество заявленных вакансий;Средняя заработная плата;количество просмотров соискателями;" +
                   "Количество размещенных резюме;Средняя желаемая заработная плата;Напряженность на рынке труда");
      foreach (var v in statistic)
      {
        sw.WriteLine($"{v.ProfessionalGroup};{v.VacancyQuantity};{v.VacancyAverageSalary};{v.VacancyViews};" +
                     $"{v.VacancyQuantity};{v.ResumeAverageSalary};");
      }
      
      sw.WriteLine($"Итог;{statisticCalculator.TotalVacancies};{statisticCalculator.TotalSalaryAverage};{statisticCalculator.TotalViews};" +
                   $"{statisticCalculator.TotalResume};{statisticCalculator.AverageSalaryResume}");
    }
  }
}