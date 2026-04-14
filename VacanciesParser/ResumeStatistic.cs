namespace VacanciesParser;

public class ResumeStatistic
{
  public string ProfessionalGroupName { get; set; }
  
  public int Quantity { get; set; }
  
  public int AverageSalary { get; set; }

  public ResumeStatistic(string professionalGroupName, int quantity, int averageSalary)
  {
    this.ProfessionalGroupName = professionalGroupName;
    
    this.Quantity = quantity;
    
    this.AverageSalary = averageSalary;
  }
}