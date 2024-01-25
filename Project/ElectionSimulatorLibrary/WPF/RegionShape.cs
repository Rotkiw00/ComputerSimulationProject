using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace ElectionSimulatorLibrary.WPF;

public class RegionShape
{
    public List<Polygon> Polygons { get; set; } = new();
    public Region? Region { get; set; }
    public int? RegionId => Region?.RegionId;
    public string? RegionName => Region?.Name;
    public SimMap.RegionClicked? OnClick { get; set; }
    public MapMode MapMode { get; set; } = MapMode.Normal;
    public Color Color { get; set; } = Color.Red;
    public double StrokeThickness { get; set; } = 1;
    private Brush ColorBrush { get; set; } = Brushes.Red;

    // TODO result mode
    public void Setup()
    {
        ColorBrush = new SolidColorBrush(System.Windows.Media.Color
            .FromArgb(255, Color.R, Color.G, Color.B));

        if (Polygons != null)
            foreach (var polygon in Polygons)
            {
                polygon.Stroke = Brushes.Black;
                polygon.StrokeThickness = StrokeThickness;

                if (MapMode == MapMode.Normal)
                {
                    polygon.Fill = Brushes.LightGray;
                }
                else
                {
                    polygon.Fill = ColorBrush;
                }
                polygon.IsMouseDirectlyOverChanged += (object sender, DependencyPropertyChangedEventArgs e) =>
                {
                    if ((bool)e.NewValue == true)
                    {
                        HoverIn();
                    }
                    else
                    {
                        HoverOut();
                    }

                };
                polygon.MouseUp += (object sender, MouseButtonEventArgs e) =>
                {
                    if (OnClick != null && RegionId != null)
                        OnClick.Invoke((int)RegionId);
                };
            }
    }
    public void HoverIn()
    {
        if (MapMode == MapMode.Normal)
        {
            if (Polygons != null)
                foreach (var polygon in Polygons)
                {
                    polygon.Fill = ColorBrush;
                }
        }
        else
        {
            // TODO result mode
        }
    }

    public void HoverOut()
    {
        if (MapMode == MapMode.Normal)
        {
            if (Polygons != null)
                foreach (var polygon in Polygons)
                {
                    polygon.Fill = Brushes.LightGray;
                }
        }
        else
        {
            // TODO result mode
        }
    }

    public void ChangeColor()
    {
        // TODO result mode
    }
}
