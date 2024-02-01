using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class Environment
{
    public Values Values { get; set; } = new Values();
    public TimeManager TimeManager { get; set; } = new();

    public List<PoliticianLeader> PoliticianLeaders { get; set; } = new();
    public List<List<Politician>> Politicians { get; set; } = new();
    public List<List<Voter>> Voters { get; set; } = new();

    public List<List<int>> Inhabited { get; set; } = new();

    public List<List<Event>> DistrictEvents { get; set; } = new();

    public List<List<Result>> CurrentResult { get; set; } = new();

    public Environment(DemographySettings demography, PoliticalSettings political, string directoryName)
    {
        Values.Demography = demography;
        Values.Political = political;
        Values.DirectoryName = directoryName;
    }

    public async Task Calculate()
    {
        #region CitizenNumberCalculation
        List<Dictionary<int, RegionType>> regionTypesList = GetInhabitableRegions();

        foreach (var district in regionTypesList)
        {
            var tmp_district = new List<int>();
            foreach (var id in district.Keys)
            {
                tmp_district.Add(id);
            }
        }

        var citizenCalculation = CalculateCitizens(regionTypesList);
        #endregion

        GeneratePoliticalLeaders();

        GeneratePoliticians();

        #region GenerateVoters
        for (int i = 0; i < 100; i++)
        {
            List<int> districtInhabited = new();
            foreach (var region in regionTypesList[i])
            {
                districtInhabited.Add(region.Key);
            }
            Inhabited.Add(districtInhabited);
            DistrictEvents.Add(new());
            Voters.Add(new());
            CurrentResult.Add(new());

            GenerateVoters(i, citizenCalculation[i]);
        }
        #endregion

        TimeManager.Reset();

        MainLoop();
    }

    public void MainLoop()
    {
        while (!TimeManager.Done)
        {
            Console.WriteLine($"Current time: {this.TimeManager.CurrentTime}");

            this.Values.GlobalResults.Add(TimeManager.CurrentTime, new());

            Random r = new Random();

            var tmpArrL = PoliticianLeaders.ToArray();
            r.Shuffle(tmpArrL);
            PoliticianLeaders = tmpArrL.ToList();

            foreach (var leader in PoliticianLeaders)
            {
                if (!leader.Available)
                {
                    leader.LockTime--;
                    if (leader.LockTime == 0)
                        leader.Available = true;
                    else continue;
                }

                int hour = BaseValues.TimeToHour(TimeManager.CurrentTime);

                if (hour >= 0 && hour < 8)
                {
                    continue;
                }

                var districtId = r.Next(100);
                var action = Action.Get(AgentType.PoliticianLeader);

                var actionResult = action.Do(leader);

                DistrictEvents[districtId].Add(actionResult);
            }

            #region DistrictLoops
            for (int i = 0; i < 100; i++)
            {
                DistrictLoop(i);
                Console.WriteLine($"District: {i}");
            }
            #endregion

            SaveGlobal(ElectionType.Sejm);

            TimeManager.Forward();
        }
    }

    public void DistrictLoop(int districtId)
    {
        Random r = new Random();

        var tmpArrP = Politicians[districtId].ToArray();
        r.Shuffle(tmpArrP);
        Politicians[districtId] = tmpArrP.ToList();

        var politicianList = Politicians[districtId];

        foreach (var politician in politicianList)
        {
            if (!politician.Available)
            {
                politician.LockTime--;
                if (politician.LockTime == 0)
                    politician.Available = true;
                else continue;
            }

            int hour = BaseValues.TimeToHour(TimeManager.CurrentTime);

            if (hour >= 0 && hour < 8)
            {
                continue;
            }

            var action = Action.Get(AgentType.Politician);

            var actionResult = action.Do(politician);

            DistrictEvents[districtId].Add(actionResult);
        }

        var tmpArrV = Voters[districtId].ToArray();
        r.Shuffle(tmpArrV);
        Voters[districtId] = tmpArrV.ToList();

        var votersList = Voters[districtId];

        foreach (var voter in votersList)
        {
            if (!voter.Available)
            {
                voter.LockTime--;
                if (voter.LockTime == 0)
                    voter.Available = true;
                else continue;
            }

            int hour = BaseValues.TimeToHour(TimeManager.CurrentTime);

            if (hour >= 0 && hour < 8)
            {
                continue;
            }

            if (voter.IsWorking && hour >= 8 && hour < 16)
            {
                continue;
            }

            double fate = r.NextDouble();

            if (fate <= this.Values.Demography.EventParticipationRate)
            {
                var tmpE = DistrictEvents[districtId].ToArray();
                r.Shuffle(tmpE);
                DistrictEvents[districtId] = tmpE.ToList();

                if (DistrictEvents[districtId].Count != 0)
                {
                    var selectedEvent = DistrictEvents[districtId][0];

                    selectedEvent.Participate(voter);

                    if (selectedEvent.EmptySlots == 0)
                    {
                        DistrictEvents[districtId].RemoveAll(e => e.EmptySlots == 0);
                        continue;
                    }
                }
            }

            var action = Action.Get(AgentType.Voter);

            var actionResult = action.Do(voter);
        }

        GenerateCurrentResult(districtId);
        SaveResult(districtId + 1, districtId);

        DistrictEvents[districtId].Clear();
        CurrentResult[districtId].Clear();
    }
        
    public void GenerateCurrentResult(int i)
    {
        List <Result> districtResult = new List <Result>();
        var d_Inhabited = Inhabited[i];

        foreach (var regionId in d_Inhabited)
        {
            var regionVoters = Voters[i].Where(v => v.RegionId == regionId).ToList();

            Result result = new Result();
            result.RegionId = regionId;

            foreach (var party in this.Values.Political.PartyList)
            {
                var favouriteCount = regionVoters
                    .Where(v => v.Score.OrderByDescending(d => d.Value).First().Key == party)
                    .ToList().Count();
                result.Popularity.Add(party, favouriteCount);
            }
            CurrentResult[i].Add(result);


        }


    }

    public Result SaveResult(int regionId, int globalId)
    {
        var json = File.ReadAllText($"MapData/{regionId}.json");
        var region = JsonSerializer.Deserialize<Region>(json);

        var result = new Result();
        result.RegionId = region.RegionId;
        foreach (var party in this.Values.Political.PartyList)
        {
            result.Popularity.Add(party, 0);
        }

        if (region != null && region.Inner != null && region.Inner.Count != 0)
        {
            foreach (var subRegion in region.Inner)
            {
                var data = SaveResult(subRegion, globalId);
                foreach (var key in data.Popularity.Keys)
                {
                    result.Popularity[key] += data.Popularity[key];
                }
            }
        }
        else
        {
            result = CurrentResult[globalId]
                .Where(r => r.RegionId == regionId)
                .ToList().FirstOrDefault();
        }

        //f (!Directory.Exists($"{this.Values.DirectoryName}/{TimeManager.CurrentTime}"))
        //{
        //    Directory.CreateDirectory($"{this.Values.DirectoryName}/{TimeManager.CurrentTime}");
        //}

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            IncludeFields = true,
        };

        //string savedJson = JsonSerializer.Serialize(result, options);
        //File.WriteAllText(Path.Combine($"{this.Values.DirectoryName}/{TimeManager.CurrentTime}", $"{regionId}.json"), savedJson);

        this.Values.GlobalResults[TimeManager.CurrentTime].Add($"{regionId}", result);

        return result;
    }

    public void SaveGlobal(ElectionType type)
    {
        var districts = new List<Region>();
        for (int i = 1; i <= 100; i++)
        {
            var json = File.ReadAllText($"MapData/{i}.json");
            var region = JsonSerializer.Deserialize<Region>(json);

            districts.Add(region);
        }

        var finalRegions = new List<Region>();
        if (type == ElectionType.Senat) finalRegions = districts;
        else
        {
            for (int i = 1; i <= 41; i++)
            {
                var regionSejm = new Region();
                regionSejm.Inner = new();
                regionSejm.SenatDistrictId = i;

                regionSejm.ElectionType = type;
                regionSejm.RegionId = i;

                var tmp = districts.Where(d => d.SejmDistrictId == i).ToList();

                foreach (var item in tmp)
                {
                    foreach (var inner in item.Inner)
                    {
                        regionSejm.Inner.Add(inner);
                    }
                }

                finalRegions.Add(regionSejm);

                var sejmResult = new Result();
                sejmResult.RegionId = regionSejm.RegionId;

                foreach (var party in this.Values.Political.PartyList)
                {
                    sejmResult.Popularity.Add(party, 0);
                }

                if (regionSejm != null && regionSejm.Inner != null && regionSejm.Inner.Count != 0)
                {
                    foreach (var subRegion in regionSejm.Inner)
                    {
                        //var jsonI = File.ReadAllText($"{this.Values.DirectoryName}/{TimeManager.CurrentTime}/{subRegion}.json");
                        //var resultI = JsonSerializer.Deserialize<Result>(jsonI);

                        var resultI = this.Values.GlobalResults[TimeManager.CurrentTime][$"{subRegion}"];

                        foreach (var key in resultI.Popularity.Keys)
                        {
                            sejmResult.Popularity[key] += resultI.Popularity[key];
                        }
                    }

                    JsonSerializerOptions options = new JsonSerializerOptions()
                    {
                        IncludeFields = true,
                    };

                    //string savedJson = JsonSerializer.Serialize(sejmResult, options);
                    string savedJson2 = JsonSerializer.Serialize(regionSejm, options);

                    //File.WriteAllText(Path.Combine($"{this.Values.DirectoryName}/{TimeManager.CurrentTime}", $"s{sejmResult.RegionId}.json"), savedJson);
                    File.WriteAllText($"MapData/s{regionSejm.RegionId}.json", savedJson2);

                    this.Values.GlobalResults[TimeManager.CurrentTime].Add($"s{sejmResult.RegionId}", sejmResult);
                }
            }


        }

    }

    public List<Dictionary<int, RegionType>> GetInhabitableRegions()
    {
        List<Dictionary<int, RegionType>> inhabitable = new();

        for (int i = 0; i < 100; i++)
        {
            inhabitable.Add(new Dictionary<int, RegionType>());
        }

        BaseValues.LoadDataFromFile("RegionsData.map");

        int fileCount = Directory.GetFiles("MapData").Count();

        for (int i = 1; i <= fileCount; i++)
        {
            var json = File.ReadAllText($"MapData/{i}.json");
            var region = JsonSerializer.Deserialize<Region>(json);

            if (region.Inhabited)
                inhabitable[region.SenatDistrictId - 1].Add(region.RegionId, region.Type);

            Console.WriteLine(region.RegionId); // TODO delete
        }




        return inhabitable;
    }

    public List<Dictionary<int, int>> CalculateCitizens(List<Dictionary<int, RegionType>> typesList)
    {
        List<Dictionary<int, int>> list = new();

        for (int i = 0; i < 100; i++)
        {
            Dictionary<int, int> element = new();
            int sum = 0;
            int all = this.Values.Demography.PopulationPerDistrict[i].Item1;
            int remaining = this.Values.Demography.PopulationPerDistrict[i].Item1;

            foreach (var item in typesList[i])
            {
                sum += this.Values.Demography.PlaceWeight[item.Value];
            }

            foreach (var item in typesList[i])
            {
                int number = (int)((this.Values.Demography.PlaceWeight[item.Value] / (double)sum) * all);
                remaining -= number;
                element.Add(item.Key, number);
            }

            if (remaining > 0)
            {
                var key = element.ElementAt(element.Count - 1).Key;
                element[key] += remaining;
            }

            list.Add(element);
        }
        return list;
    }

    public void GeneratePoliticalLeaders()
    {
        var parties = this.Values.Political.PartyList;

        foreach (var party in parties)
        {
            var leader = new PoliticianLeader();

            Random r = new Random();
            leader.Party = party;

            int cp = r.Next(party.Views.Conservatism_Progressivism - 5, party.Views.Conservatism_Progressivism + 5);
            int ee = r.Next(party.Views.Euroscepticism_Euroenthusiasm - 5, party.Views.Euroscepticism_Euroenthusiasm + 5);
            int sc = r.Next(party.Views.Socialism_Capitalism - 5, party.Views.Socialism_Capitalism + 5);
            int il = r.Next(party.Views.IlliberalDemocracy_LiberalDemocracy - 5, party.Views.IlliberalDemocracy_LiberalDemocracy + 5);

            leader.Views.Conservatism_Progressivism = cp;
            leader.Views.Euroscepticism_Euroenthusiasm = ee;
            leader.Views.Socialism_Capitalism = sc;
            leader.Views.IlliberalDemocracy_LiberalDemocracy = il;

            PoliticianLeaders.Add(leader);
        }
    }

    public void GeneratePoliticians()
    {
        var parties = this.Values.Political.PartyList;

        for (int i = 0; i < 100; i++)
        {
            var districtList = new List<Politician>();

            foreach (var party in parties)
            {
                Region tmp = new Region();
                tmp.SenatDistrictId = i + 1;
                int number = ((BaseValues.GetSejmMandates(tmp.SejmDistrictId) / 3) * 2) + 1;

                for (int j = 0; j < number; j++)
                {
                    var politician = new Politician();

                    Random r = new Random();
                    politician.Party = party;

                    int cp = r.Next(party.Views.Conservatism_Progressivism - 10, party.Views.Conservatism_Progressivism + 10);
                    int ee = r.Next(party.Views.Euroscepticism_Euroenthusiasm - 10, party.Views.Euroscepticism_Euroenthusiasm + 10);
                    int sc = r.Next(party.Views.Socialism_Capitalism - 10, party.Views.Socialism_Capitalism + 10);
                    int il = r.Next(party.Views.IlliberalDemocracy_LiberalDemocracy - 10, party.Views.IlliberalDemocracy_LiberalDemocracy + 10);

                    politician.Views.Conservatism_Progressivism = cp;
                    politician.Views.Euroscepticism_Euroenthusiasm = ee;
                    politician.Views.Socialism_Capitalism = sc;
                    politician.Views.IlliberalDemocracy_LiberalDemocracy = il;

                    districtList.Add(politician);
                }
            }

            Politicians.Add(districtList);
        }


    }

    public void GenerateVoters(int district, Dictionary<int, int> calculations)
    {
        List<Voter> districtList = new List<Voter>();

        for (int i = 0; i < calculations.Count; i++)
        {
            var calc = calculations.ElementAt(i);

            for (int j = 0; j < calculations[calc.Key]; j++)
            {
                var voter = new Voter();
                voter.Score = new();
                voter.RegionId = calc.Key;

                Random r = new Random();

                double fate = r.NextDouble();
                voter.IsWorking = fate <= 60;

                int cp = r.Next(-100, 100);
                int ee = r.Next(-100, 100);
                int sc = r.Next(-100, 100);
                int il = r.Next(-100, 100);

                voter.Views.Conservatism_Progressivism = cp;
                voter.Views.Euroscepticism_Euroenthusiasm = ee;
                voter.Views.Socialism_Capitalism = sc;
                voter.Views.IlliberalDemocracy_LiberalDemocracy = il;

                districtList.Add(voter);
            }


        }

        // CONTROL VALUE
        int numberToProcess = 100000;
        int allowedDifference = 10;

        var regionViews = this.Values.Demography.PopulationPerDistrict[district].Item2;

        //Conservatism_Progressivism
        while (BaseValues.PopulationDifference(regionViews.Conservatism_Progressivism, districtList, 1) > allowedDifference)
        {
            var tmpArr = districtList.ToArray();
            Random r = new Random();
            r.Shuffle(tmpArr);
            districtList = tmpArr.ToList();
            for (int i = 0; i < numberToProcess; i++)
            {
                var agent = districtList[i];

                if (agent.Views.Conservatism_Progressivism > regionViews.Conservatism_Progressivism)
                    agent.Views.Conservatism_Progressivism -= agent.Views.Conservatism_Progressivism - regionViews.Conservatism_Progressivism < 10 ? 10 : agent.Views.Conservatism_Progressivism - regionViews.Conservatism_Progressivism;
                else
                    agent.Views.Conservatism_Progressivism += regionViews.Conservatism_Progressivism - agent.Views.Conservatism_Progressivism < 10 ? 10 : regionViews.Conservatism_Progressivism - agent.Views.Conservatism_Progressivism;
            }
        }
        //Euroscepticism_Euroenthusiasm
        while (BaseValues.PopulationDifference(regionViews.Euroscepticism_Euroenthusiasm, districtList, 2) > allowedDifference)
        {
            var tmpArr = districtList.ToArray();
            Random r = new Random();
            r.Shuffle(tmpArr);
            districtList = tmpArr.ToList();
            for (int i = 0; i < numberToProcess; i++)
            {
                var agent = districtList[i];

                if (agent.Views.Euroscepticism_Euroenthusiasm > regionViews.Euroscepticism_Euroenthusiasm)
                    agent.Views.Euroscepticism_Euroenthusiasm -= agent.Views.Euroscepticism_Euroenthusiasm - regionViews.Euroscepticism_Euroenthusiasm < 10 ? 10 : agent.Views.Euroscepticism_Euroenthusiasm - regionViews.Euroscepticism_Euroenthusiasm;
                else
                    agent.Views.Euroscepticism_Euroenthusiasm += regionViews.Euroscepticism_Euroenthusiasm - agent.Views.Euroscepticism_Euroenthusiasm < 10 ? 10 : regionViews.Euroscepticism_Euroenthusiasm - agent.Views.Euroscepticism_Euroenthusiasm;
            }
        }
        //Socialism_Capitalism
        while (BaseValues.PopulationDifference(regionViews.Socialism_Capitalism, districtList, 3) > allowedDifference)
        {
            var tmpArr = districtList.ToArray();
            Random r = new Random();
            r.Shuffle(tmpArr);
            districtList = tmpArr.ToList();
            for (int i = 0; i < numberToProcess; i++)
            {
                var agent = districtList[i];

                if (agent.Views.Socialism_Capitalism > regionViews.Socialism_Capitalism)
                    agent.Views.Socialism_Capitalism -= agent.Views.Socialism_Capitalism - regionViews.Socialism_Capitalism < 10 ? 10 : agent.Views.Socialism_Capitalism - regionViews.Socialism_Capitalism;
                else
                    agent.Views.Socialism_Capitalism += regionViews.Socialism_Capitalism - agent.Views.Socialism_Capitalism < 10 ? 10 : regionViews.Socialism_Capitalism - agent.Views.Socialism_Capitalism;
            }
        }
        //IlliberalDemocracy_LiberalDemocracy
        while (BaseValues.PopulationDifference(regionViews.IlliberalDemocracy_LiberalDemocracy, districtList, 4) > allowedDifference)
        {
            var tmpArr = districtList.ToArray();
            Random r = new Random();
            r.Shuffle(tmpArr);
            districtList = tmpArr.ToList();
            for (int i = 0; i < numberToProcess; i++)
            {
                var agent = districtList[i];

                if (agent.Views.IlliberalDemocracy_LiberalDemocracy > regionViews.IlliberalDemocracy_LiberalDemocracy)
                    agent.Views.IlliberalDemocracy_LiberalDemocracy -= agent.Views.IlliberalDemocracy_LiberalDemocracy - regionViews.IlliberalDemocracy_LiberalDemocracy < 10 ? 10 : agent.Views.IlliberalDemocracy_LiberalDemocracy - regionViews.IlliberalDemocracy_LiberalDemocracy;
                else
                    agent.Views.IlliberalDemocracy_LiberalDemocracy += regionViews.IlliberalDemocracy_LiberalDemocracy - agent.Views.IlliberalDemocracy_LiberalDemocracy < 10 ? 10 : regionViews.IlliberalDemocracy_LiberalDemocracy - agent.Views.IlliberalDemocracy_LiberalDemocracy;
            }
        }

        foreach (var voter in districtList)
        {
            foreach (var party in this.Values.Political.PartyList)
            {
                var difference = voter.Views.CalculateDifference(party.Views);
                voter.Score.Add(party, difference * 10);
            }
        }

        Voters[district] = districtList;

        Console.WriteLine($"Generated voters - {district}");
    }
}
