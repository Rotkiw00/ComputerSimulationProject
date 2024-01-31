using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectionSimulatorLibrary;

public class Event
{
    public EventType Type { get; set; } = EventType.Single;
    public Attributes Views { get; set; } = new(0, 0, 0, 0);
    public int EmptySlots { get; set; } = 1;
    public PoliticalParty Party { get; set; }
    public int EventDuration { get; set; } = 1;

    public double IncreaseSupportValue { get; set; } = 1;
    public double DecreaseSupportValue { get; set; } = 1;
    public double IncreaseSupportChance { get; set; } = 0.5;
    public double DecreaseSupportChance { get; set; } = 0.5;

    public void Participate(Voter agent)
    {
        agent.LockTime += EventDuration;

        Random r = new Random();
        double fate = 0;

        double difference = agent.Views.CalculateDifference(this.Views);

        fate = r.NextDouble();
        if (IncreaseSupportChance >= fate)
        {
            agent.Score[Party] += (IncreaseSupportValue * ((100 - difference) / 100.0));
        }

        fate = r.NextDouble();
        if (DecreaseSupportChance >= fate)
        {
            agent.Score[Party] -= (DecreaseSupportValue * (difference / 100.0));
        }

        EmptySlots--;
    }
}
