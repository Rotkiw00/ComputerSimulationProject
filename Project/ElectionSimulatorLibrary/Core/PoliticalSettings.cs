using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class PoliticalSettings
{
    public List<PoliticalParty> PartyList { set; get; } = new();

    public void Default()
    {
        PartyList.Add(new PoliticalParty
        {
            Name = "Black",
            Color = Color.Black,
            Views = new(-70, -65, -70, -80)
        });
        PartyList.Add(new PoliticalParty
        {
            Name = "Blue",
            Color = Color.Blue,
            Views = new(70, 80, 55, 80)
        });
        PartyList.Add(new PoliticalParty
        {
            Name = "Green",
            Color = Color.Green,
            Views = new(-30, 65, 70, 70)
        });
        PartyList.Add(new PoliticalParty
        {
            Name = "Pink",
            Color = Color.Pink,
            Views = new(90, 90, -60, 75)
        });
        PartyList.Add(new PoliticalParty
        {
            Name = "Red",
            Color = Color.Red,
            Views = new(-90, -90, 90, -40)
        });
    }

    public void Clear()
    {
        PartyList.Clear();
    }
}
