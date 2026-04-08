namespace VacanciesParser;

public class VacancyStatistic
{
  public string ProfessionalGroup { get; set; }
  
  public int Quantity { get; set; }
  
  public int AverageSalary { get; set; }
  
  public int Views { get; set; }

  public VacancyStatistic(string professionalGroup, int quantity, int averageSalary, int views)
  {
    this.ProfessionalGroup = professionalGroup;
    
    this.Quantity = quantity;
    
    this.AverageSalary = averageSalary;
    
    this.Views = views;
  }
}