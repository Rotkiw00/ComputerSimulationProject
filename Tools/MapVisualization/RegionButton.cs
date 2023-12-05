using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WebScraper;

namespace MapVisualization;

public class RegionButton : System.Windows.Controls.Button
{
    public delegate void RegionButtonClick();

    private List<Polygon>? _polygons;
    private Region? _region;
    private Action? _click;

    public RegionButton(List<Polygon> polygons, Region region, Action click)
    {
        _polygons = polygons;
        _region = region;
        _click = click;

        if(_polygons == null || _region == null || _click == null || _region.Inner == null)
        {
            this.IsEnabled = false;
        }

        if(_region != null)
        {
            this.Content = _region.Name;
        }
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        if (_polygons != null)
            foreach (var p in _polygons)
            {
                p.Fill = Brushes.Red;
            }
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        if (_polygons != null)
            foreach (var p in _polygons)
            {
                p.Fill = Brushes.LightGray;
            }
    }

    protected override void OnClick()
    {
        base.OnClick();

        _click?.Invoke();
    }
}
