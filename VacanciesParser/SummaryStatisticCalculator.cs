namespace VacanciesParser;

public class SummaryStatisticCalculator
{
  public int TotalVacancies { get; set; }
  
  public double TotalSalaryAverage { get; set; }
  
  public int TotalViews { get; set; }
  
  public int TotalResume { get; set; }
  
  public double AverageSalaryResume { get; set; }
  
  public double AverageIntensity { get; set; }

  public SummaryStatisticCalculator(int totalVacancies, double totalSalaryAverage, int totalViews,
                                    int totalResume, double averageSalaryResume, double averageIntensity)
  {
    this.TotalVacancies = totalVacancies;
    
    this.TotalSalaryAverage = totalSalaryAverage;
    
    this.TotalViews = totalViews;
    
    this.TotalResume = totalResume;
    
    this.AverageSalaryResume = averageSalaryResume;
    
    this.AverageIntensity = averageIntensity;
  }
}