using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class Result
{
    public int RegionId {  get; set; }
    public bool Final {  get; set; } = false;
    public Dictionary<PoliticalParty, double> Popularity { get; set; } = new();
    public List<(PoliticalParty, int)> Mandates { get; set; } = new();
}
