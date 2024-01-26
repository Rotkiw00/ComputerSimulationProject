using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class Action
{
    ActionType Type { get; set; } = ActionType.A_DoNothing;
    public double RangeStart { get; set; } = 0;
    public double RangeEnd { get; set; } = 1;


}
