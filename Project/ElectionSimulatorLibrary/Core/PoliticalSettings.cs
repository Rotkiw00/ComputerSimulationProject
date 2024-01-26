using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class PoliticalSettings
{
    public List<(PoliticalParty, double)> PartyList { set; get; } = new();

    public void Normalize()
    {
        double sum = 0;
        foreach (var party in PartyList)
        {
            sum += party.Item2;
        }
        for (int i = 0; i < PartyList.Count; i++)
        {
            double normalized = PartyList[i].Item2 / sum;
            PartyList[i] = (PartyList[i].Item1, normalized);
        }
    }
    public bool Check()
    {
        double sum = 0;
        foreach (var party in PartyList)
        {
            sum += party.Item2;
        }

        return sum <= 1;
    }
}
