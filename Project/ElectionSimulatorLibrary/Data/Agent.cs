using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public abstract class Agent
{
    public Attributes Views { get; set; } = new(0, 0, 0, 0);
    public int LockTime { get; set; } = 0;
    public bool Available { get; set; } = true;

    public abstract PoliticalParty GetTopParty();

}
