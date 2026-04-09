using ScottPlot;

namespace VacanciesParser;

public class DiagramService
{
  public void WriteVacanciesCircleDiagram(List<VacancyStatistic> vacanciesStatistic, string path)
  {
    string fullPath = $"{path}/professional_groups.png";
    
    Plot plot = new();
    
    double[] values = new double[vacanciesStatistic.Count];

    for (int i = 0; i < vacanciesStatistic.Count; i++)
    {
      values[i] = vacanciesStatistic[i].Quantity;
    }
    
    var pie = plot.Add.Pie(values);
    //pie.ExplodeFraction = 0.1;
    pie.SliceLabelDistance = 0.5;

// set different labels for slices and legend
    double total = pie.Slices.Select(x => x.Value).Sum();
    for (int i = 0; i < pie.Slices.Count; i++)
    {
      pie.Slices[i].LabelFontSize = 20;
      pie.Slices[i].Label = $"{pie.Slices[i].Value}";
      pie.Slices[i].LegendText = $"{vacanciesStatistic[i].ProfessionalGroup} " +
                                 $"({pie.Slices[i].Value / total:p1})";
    }

// hide unnecessary plot components
    plot.Axes.Frameless();
    plot.HideGrid();
    
    /*Plot plot = new Plot();
    
    string fullPath = $"{path}/professional_groups.png";

    var colors = new[]
    {
      Colors.Red,
      Colors.Green,
      Colors.Yellow,
      Colors.Blue,
      Colors.Orange,
      Colors.Purple
    };

    List<PieSlice> slices = new List<PieSlice>();

    for (int i = 0; i < vacanciesStatistic.Count; i++)
    {
      slices.Add(new PieSlice()
      {
        Value = vacanciesStatistic[i].Quantity,
        FillColor = colors[i],
        Label = vacanciesStatistic[i].Quantity.ToString(),
        LegendText = vacanciesStatistic[i].ProfessionalGroup
      });
    }
    
    plot.Legend.MarkerShapeOverride = MarkerShape.FilledCircle;

    var pie = plot.Add.Pie(slices);

    plot.Axes.Frameless();
    plot.HideGrid();
    plot.ShowLegend();*/

    Console.WriteLine($"save circle diagram to {fullPath}");
    plot.SavePng(fullPath, 1000, 800);
  }

  public void WriteSalaryLineDiagram(List<VacancyStatistic> vacanciesStatistic, string path)
  {
    Plot plot = new Plot();
    
    string fullPath = $"{path}/professional_groups_salary.png";
    
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