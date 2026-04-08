namespace VacanciesParser;

public class VacancyStatisticSample
{
  public string ProfessionalGroup { get; set; }
  
  public int AverageSalary { get; set; }
  
  public int VacancyViews { get; set; }
  
  public VacancyStatisticSample(string professionalGroup, int averageSalary, int vacancyViews)
  {
    this.ProfessionalGroup = professionalGroup;
    
    this.AverageSalary = averageSalary;
    
    this.VacancyViews = vacancyViews;
  }
}