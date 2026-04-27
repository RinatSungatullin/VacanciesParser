using ScottPlot;

namespace VacanciesParser;

public class DiagramService
{
  public void WriteProfessionalGroupsCircleDiagram(List<VacancyStatistic> vacanciesStatistic, string path, string fileName)
  {
    string fullPath = $"{path}/{fileName}.png";
    
    Plot plot = new();
    
    double[] values = new double[vacanciesStatistic.Count];

    for (int i = 0; i < vacanciesStatistic.Count; i++)
    {
      values[i] = vacanciesStatistic[i].Quantity;
    }
    
    var pie = plot.Add.Pie(values);
    pie.SliceLabelDistance = 0.5;
    
    double total = pie.Slices.Select(x => x.Value).Sum();
    for (int i = 0; i < pie.Slices.Count; i++)
    {
      pie.Slices[i].LabelFontSize = 20;
      pie.Slices[i].Label = $"{pie.Slices[i].Value}";
      pie.Slices[i].LegendText = $"{vacanciesStatistic[i].ProfessionalGroup} " +
                                 $"({pie.Slices[i].Value / total:p1})";
    }

    plot.Axes.Frameless();
    plot.HideGrid();

    Console.WriteLine($"save circle diagram to {fullPath}");
    plot.SavePng(fullPath, 1000, 800);
  }

  public void WriteSalaryLineDiagram(List<VacancyStatistic> vacanciesStatistic, string path, string fileName)
  {
    Plot plot = new Plot();
    
    string fullPath = $"{path}/{fileName}.png";
    
    double[] values = new double[vacanciesStatistic.Count];

    for (int i = 0; i < vacanciesStatistic.Count; i++)
    {
      values[i] = vacanciesStatistic[i].AverageSalary;
    }
    
    var barPlot = plot.Add.Bars(values);

    foreach (var bar in barPlot.Bars)
    {
      bar.Label = $"{bar.Value.ToString()} Руб.";
    }
    
    plot.Axes.Margins(bottom: 0);

    Tick[] ticks = new Tick[values.Length];

    for (int i = 0; i < ticks.Length; i++)
    {
      ticks[i] = new Tick(i, vacanciesStatistic[i].ProfessionalGroup);
    }
    plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
    plot.Axes.Bottom.TickLabelStyle.Rotation = 45;
    plot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleLeft;

    float largestLabelWidth = 0;
    using Paint paint = Paint.NewDisposablePaint();
    foreach (Tick tick in ticks)
    {
      PixelSize size = plot.Axes.Bottom.TickLabelStyle.Measure(tick.Label, paint).Size;
      largestLabelWidth = Math.Max(largestLabelWidth, size.Width);
    }

    plot.Axes.Bottom.MinimumSize = largestLabelWidth;
    plot.Axes.Right.MinimumSize = largestLabelWidth;

    Console.WriteLine($"save line diagram to {fullPath}");
    plot.SavePng(fullPath, 1000, 800);
  }
}