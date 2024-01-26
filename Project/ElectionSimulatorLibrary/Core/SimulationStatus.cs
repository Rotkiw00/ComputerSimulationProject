using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class SimulationStatus
{
    public ResponseCode Response { get; set; } = ResponseCode.Undefined;
    public object Data { get; set; } = "";
}
