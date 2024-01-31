using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class Simulation
{

    public Simulation(DemographySettings demography, PoliticalSettings political)
    {

    }

    public SimulationStatus Start()
    {
        return new SimulationStatus { Response = ResponseCode.Undefined, Data = "" };
    }

}
