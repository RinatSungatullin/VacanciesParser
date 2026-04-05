using System.Text;

namespace VacanciesParser;

public class FileService
{
  public void WriteVacanciesToCsv(List<VacancyWrapper> vacancies, string filePath, string fileName)
  {
    string fullPath = Path.Combine(filePath, $"{fileName}.csv");

    using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8))
    {
      sw.WriteLine("vacancy_id;vacancy_name;salary_from;salary_to;vacancy_url;specialization;education");

      foreach (var v in vacancies)
      {
        sw.WriteLine($"{v.Vacancy.Id};{v.Vacancy.JobName};{v.Vacancy.SalaryMin};{v.Vacancy.SalaryMax};" +
                     $"{v.Vacancy.VacancyUrl};{v.Vacancy.Category.Specialisation};{v.Vacancy.Requirement.Education}");
      }
    }
  }
}