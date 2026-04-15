namespace VacanciesParser;

public class VacancyStatisticCalculator
{
  public int TotalVacancies { get; set; }
  
  public double TotalSalaryAverage { get; set; }
  
  public int TotalViews { get; set; }
  
  public int TotalResume { get; set; }
  
  public double AverageSalaryResume { get; set; }

  public VacancyStatisticCalculator(int totalVacancies, double totalSalaryAverage, int totalViews,
                                    int totalResume, double averageSalaryResume)
  {
    this.TotalVacancies = totalVacancies;
    
    this.TotalSalaryAverage = totalSalaryAverage;
    
    this.TotalViews = totalViews;
    
    this.TotalResume = totalResume;
    
    this.AverageSalaryResume = averageSalaryResume;
  }
}