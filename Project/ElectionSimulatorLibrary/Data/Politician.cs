using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class Politician : Voter
{
    public PoliticalParty Party { get; set; }

    public override PoliticalParty GetTopParty()
    {
        return Party;
    }
}
