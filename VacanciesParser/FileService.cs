namespace VacanciesParser;

public class FileService
{
  public void WriteToCsv(List<Vacancy> vacancies, string filePath, string fileName)
  {
    string fullPath = Path.Combine(filePath, $"{fileName}.csv");

    /*using (var streamWriter = new StreamWriter(fullPath))
    {
      using (var csWriter = new )
      {
        
      }
    }*/
  }
}