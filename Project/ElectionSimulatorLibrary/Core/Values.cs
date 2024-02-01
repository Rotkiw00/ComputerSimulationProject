using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class Values
{
    public DemographySettings Demography { get; set; }
    public PoliticalSettings  Political { get; set; }
    public string DirectoryName { get; set; } = "Results";

    public Dictionary<int, Dictionary<string, Result>> GlobalResults { get; set; } = new();

}
