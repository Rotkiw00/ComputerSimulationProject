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
            if (_possibleActions != null && _possibleActions.Count != 0)
                return _possibleActions;

            _possibleActions = new List<Action>();

            // ** CONTROL VALUES **
            #region ActionDefiniton
            _possibleActions.Add(new Action
            {
                Type = ActionType.PL_Meeting,
                AgentType = AgentType.PoliticianLeader,
                RangeStart = 0,
                RangeEnd = 0.4,
                IncreaseSupportValue = 500,
                DecreaseSupportValue = 250,
                IncreaseSupportChance = 0.7,
                DecreaseSupportChance = 0.2,
                EventEmptySlots = 1000,
                ActionDuration = 2
            });
            _possibleActions.Add(new Action
            {
                Type = ActionType.PL_Live_Tv,
                AgentType = AgentType.PoliticianLeader,
                RangeStart = 0.4,
                RangeEnd = 0.6,
                IncreaseSupportValue = 250,
                DecreaseSupportValue = 150,
                IncreaseSupportChance = 0.4,
                DecreaseSupportChance = 0.3,
                EventEmptySlots = -1,
                ActionDuration = 4
            });
            _possibleActions.Add(new Action
            {
                Type = ActionType.PL_Internet,
                AgentType = AgentType.PoliticianLeader,
                RangeStart = 0.6,
                RangeEnd = 1,
                IncreaseSupportValue = 100,
                DecreaseSupportValue = 100,
                IncreaseSupportChance = 0.4,
                DecreaseSupportChance = 0.3,
                EventEmptySlots = -1,
                ActionDuration = 1
            });
            _possibleActions.Add(new Action
            {
                Type = ActionType.P_Meeting,
                AgentType = AgentType.Politician,
                RangeStart = 0,
                RangeEnd = 0.4,
                IncreaseSupportValue = 300,
                DecreaseSupportValue = 100,
                IncreaseSupportChance = 0.6,
                DecreaseSupportChance = 0.2,
                EventEmptySlots = 250,
                ActionDuration = 2
            });
            _possibleActions.Add(new Action
            {
                Type = ActionType.P_Live_Tv,
                AgentType = AgentType.Politician,
                RangeStart = 0.4,
                RangeEnd = 0.6,
                IncreaseSupportValue = 200,
                DecreaseSupportValue = 100,
                IncreaseSupportChance = 0.4,
                DecreaseSupportChance = 0.3,
                EventEmptySlots = -1,
                ActionDuration = 4
            });
            _possibleActions.Add(new Action
            {
                Type = ActionType.P_Internet,
                AgentType = AgentType.Politician,
                RangeStart = 0.6,
                RangeEnd = 1,
                IncreaseSupportValue = 100,
                DecreaseSupportValue = 100,
                IncreaseSupportChance = 0.4,
                DecreaseSupportChance = 0.3,
                EventEmptySlots = -1,
                ActionDuration = 1
            });
            _possibleActions.Add(new Action
            {
                Type = ActionType.V_Live_Tv,
                AgentType = AgentType.Voter,
                RangeStart = 0,
                RangeEnd = 0.5,
                ViewsChangeUpValue = 5,
                ViewsChangeDownValue = 3,
                ViewsChangeUpChance = 0.4,
                ViewsChangeDownChance = 0.2,
                EventEmptySlots = 1,
                ActionDuration = 2
            });
            _possibleActions.Add(new Action
            {
                Type = ActionType.V_Internet,
                AgentType = AgentType.Voter,
                RangeStart = 0.5,
                RangeEnd = 1,
                ViewsChangeUpValue = 5,
                ViewsChangeDownValue = 3,
                ViewsChangeUpChance = 0.4,
                ViewsChangeDownChance = 0.2,
                EventEmptySlots = 1,
                ActionDuration = 1
            });
            #endregion

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
        if (ViewsChangeDownChance >= fate)
        {
            bool positive = agent.Views.Conservatism_Progressivism >= 0;
            if (positive) agent.Views.Conservatism_Progressivism -= ViewsChangeDownValue;
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

        if (agent is not Politician)
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
