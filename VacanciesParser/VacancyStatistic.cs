namespace VacanciesParser;

public class VacancyStatistic
{
  public string ProfessionalGroup { get; set; }
  
  public int AverageSalary { get; set; }
  
  public int VacancyViews { get; set; }
  
  public VacancyStatistic(string professionalGroup, int averageSalary, int vacancyViews)
  {
    this.ProfessionalGroup = professionalGroup;
    
    this.AverageSalary = averageSalary;
    
    this.VacancyViews = vacancyViews;
  }
}