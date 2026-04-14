namespace VacanciesParser;

public class SummaryStatistic
{
  public string ProfessionalGroup { get; set; }

  public int ResumeQuantity { get; set; }
  public int ResumeAverageSalary { get; set; }

  public int VacancyQuantity { get; set; }
  public int VacancyAverageSalary { get; set; }
  public int VacancyViews { get; set; }

  public SummaryStatistic(
    string professionalGroup,
    int resumeQuantity,
    int resumeAverageSalary,
    int vacancyQuantity,
    int vacancyAverageSalary,
    int vacancyViews)
  {
    ProfessionalGroup = professionalGroup;

    ResumeQuantity = resumeQuantity;
    ResumeAverageSalary = resumeAverageSalary;

    VacancyQuantity = vacancyQuantity;
    VacancyAverageSalary = vacancyAverageSalary;
    VacancyViews = vacancyViews;
  }
}