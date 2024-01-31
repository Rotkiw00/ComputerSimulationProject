using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class Action
{
    private static List<Action> _possibleActions;

    private static List<Action> PossibleActions 
    {
        get
        {
            if(_possibleActions != null && _possibleActions.Count != 0)
                return _possibleActions;

            _possibleActions = new List<Action>();

            // ** CONTROL VALUES **
            // TODO add actions

            return _possibleActions;
        }
    }

    public static Action Get(AgentType type)
    {
        Random r = new Random();
        double fate = r.NextDouble();

        return PossibleActions
            .Where(a => a.AgentType == type).ToList()
            .Where(a => (fate >= a.RangeStart && fate < a.RangeEnd)).ToList()
            .First();
    }

    ActionType Type { get; set; } = ActionType.A_DoNothing;
    AgentType AgentType { get; set; } = AgentType.Agent;

    public double RangeStart { get; set; } = 0; //inclusive
    public double RangeEnd { get; set; } = 1; //exclusive

    public double IncreaseSupportValue { get; set; } = 1;
    public double DecreaseSupportValue { get; set; } = 1;
    public double IncreaseSupportChance { get; set; } = 0.5;
    public double DecreaseSupportChance { get; set; } = 0.5;

    public int ViewsChangeUpValue { get; set; } = 1;
    public int ViewsChangeDownValue { get; set; } = 1;
    public double ViewsChangeUpChance { get; set; } = 0.5;
    public double ViewsChangeDownChance { get; set; } = 0.5;

    public int EventEmptySlots { get; set; } = 0;

    public int ActionDuration { get; set; } = 1;

    public Event Do(Agent agent)
    {
        agent.LockTime += ActionDuration;

        Random r = new Random();
        double fate = 0;

        #region ViewsModification
        // Conservatism_Progressivism
        fate = r.NextDouble();
        if(ViewsChangeDownChance >= fate)
        {
            bool positive = agent.Views.Conservatism_Progressivism >= 0;
            if(positive) agent.Views.Conservatism_Progressivism -= ViewsChangeDownValue;
            else agent.Views.Conservatism_Progressivism += ViewsChangeDownValue;
        }

        fate = r.NextDouble();
        if (ViewsChangeUpChance >= fate)
        {
            bool positive = agent.Views.Conservatism_Progressivism >= 0;
            if (positive) agent.Views.Conservatism_Progressivism += ViewsChangeUpValue;
            else agent.Views.Conservatism_Progressivism += ViewsChangeUpValue;
        }

        // Euroscepticism_Euroenthusiasm
        fate = r.NextDouble();
        if (ViewsChangeDownChance >= fate)
        {
            bool positive = agent.Views.Euroscepticism_Euroenthusiasm >= 0;
            if (positive) agent.Views.Euroscepticism_Euroenthusiasm -= ViewsChangeDownValue;
            else agent.Views.Euroscepticism_Euroenthusiasm += ViewsChangeDownValue;
        }

        fate = r.NextDouble();
        if (ViewsChangeUpChance >= fate)
        {
            bool positive = agent.Views.Euroscepticism_Euroenthusiasm >= 0;
            if (positive) agent.Views.Euroscepticism_Euroenthusiasm += ViewsChangeUpValue;
            else agent.Views.Euroscepticism_Euroenthusiasm += ViewsChangeUpValue;
        }

        // Socialism_Capitalism
        fate = r.NextDouble();
        if (ViewsChangeDownChance >= fate)
        {
            bool positive = agent.Views.Socialism_Capitalism >= 0;
            if (positive) agent.Views.Socialism_Capitalism -= ViewsChangeDownValue;
            else agent.Views.Socialism_Capitalism += ViewsChangeDownValue;
        }

        fate = r.NextDouble();
        if (ViewsChangeUpChance >= fate)
        {
            bool positive = agent.Views.Socialism_Capitalism >= 0;
            if (positive) agent.Views.Socialism_Capitalism += ViewsChangeUpValue;
            else agent.Views.Socialism_Capitalism += ViewsChangeUpValue;
        }

        // IlliberalDemocracy_LiberalDemocracy
        fate = r.NextDouble();
        if (ViewsChangeDownChance >= fate)
        {
            bool positive = agent.Views.IlliberalDemocracy_LiberalDemocracy >= 0;
            if (positive) agent.Views.IlliberalDemocracy_LiberalDemocracy -= ViewsChangeDownValue;
            else agent.Views.IlliberalDemocracy_LiberalDemocracy += ViewsChangeDownValue;
        }

        fate = r.NextDouble();
        if (ViewsChangeUpChance >= fate)
        {
            bool positive = agent.Views.IlliberalDemocracy_LiberalDemocracy >= 0;
            if (positive) agent.Views.IlliberalDemocracy_LiberalDemocracy += ViewsChangeUpValue;
            else agent.Views.IlliberalDemocracy_LiberalDemocracy += ViewsChangeUpValue;
        }
        #endregion

        var actionResult = new Event
        {
            Views = agent.Views,
            EmptySlots = EventEmptySlots,
            Party = agent.GetTopParty(),
            EventDuration = ActionDuration,
            IncreaseSupportValue = IncreaseSupportValue,
            IncreaseSupportChance = IncreaseSupportChance,
            DecreaseSupportValue = DecreaseSupportValue,
            DecreaseSupportChance = DecreaseSupportChance
        };

        if(agent is not Politician)
        {
            actionResult.Type = EventType.Single;
        }
        else
        {
            actionResult.Type = EventType.Multi;
        }

        return actionResult;
    }
}
