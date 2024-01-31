using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class Voter : Agent
{
    public Dictionary<PoliticalParty, double> Score { get; set; } = new();
    public int RegionId { get; set; } = 0;
    public bool IsWorking { get; set; } = true;

    public override PoliticalParty GetTopParty()
    {
        return Score.Select(s => (s.Key, s.Value))
            .ToList()
            .OrderByDescending(s => s.Value)
            .ToList()
            .FirstOrDefault()
            .Key;
    }
}
