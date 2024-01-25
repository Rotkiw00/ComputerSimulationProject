using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ElectionSimulatorLibrary.WPF;

public class RegionButton : System.Windows.Controls.Button
{
    private RegionShape? _shape;

    public RegionButton(RegionShape? shape)
    {
        _shape = shape;

        if(_shape?.Polygons == null)
        {
            this.IsEnabled = false;
        }

        if(_shape?.RegionName != null)
        {
            this.Content = _shape?.RegionName;
        }
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        if (_shape?.Polygons != null)
            _shape.HoverIn();
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        if (_shape?.Polygons != null)
            _shape.HoverIn();
    }

    protected override void OnClick()
    {
        base.OnClick();

        if (_shape?.OnClick != null && _shape?.RegionId != null)
            _shape.OnClick.Invoke((int)_shape.RegionId);
    }
}
