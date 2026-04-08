namespace VacanciesParser;

public class VacancyStatisticCalculator
{
  public int TotalVacancies { get; set; }
  
  public int TotalSalaryAverage { get; set; }
  
  public int TotalViews { get; set; }

  public VacancyStatisticCalculator(int totalVacancies, int totalSalaryAverage, int totalViews)
  {
    this.TotalVacancies = totalVacancies;
    
    this.TotalSalaryAverage = totalSalaryAverage;
    
    this.TotalViews = totalViews;
  }
}