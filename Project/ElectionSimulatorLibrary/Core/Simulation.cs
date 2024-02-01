using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class Simulation
{

    private DemographySettings _demography;
    private PoliticalSettings _political;
    private Environment env;

    public string DirectoryName { get; set; } = "Results";

    public Simulation(DemographySettings demography, PoliticalSettings political)
    {
        _demography = demography;
        _political = political;
    }

    public async Task<bool> Start()
    {
        try
        {
            if (Directory.Exists(DirectoryName))
            {
                Directory.Delete(DirectoryName, true);
            }

            Directory.CreateDirectory(DirectoryName);

            env = new Environment(_demography, _political, DirectoryName);

            await env.Calculate();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public List<Result> GetResult(int regionId, ElectionType type, int time)
    {
        List<Result> result = new List<Result>();

        if (regionId == 0)
        {
            if (type == ElectionType.Senat)
            {
                for (int i = 1; i <= 100; i++)
                {
                    var resultI = this.env.Values.GlobalResults[time][$"{i}"];
                    result.Add(resultI);
                }
            }
            else if (type == ElectionType.Sejm)
            {
                for (int i = 1; i <= 41; i++)
                {
                    var resultI = this.env.Values.GlobalResults[time][$"s{i}"];
                    result.Add(resultI);
                }
            }
        }
        else if (type == ElectionType.Sejm && regionId <= 41)
        {
            var jsonR = File.ReadAllText($"MapData/s{regionId}.json");
            var region = JsonSerializer.Deserialize<Region>(jsonR);

            if (region.Inner != null && region.Inner.Count != 0)
            {
                foreach (var inner in region.Inner)
                {
                    var jsonI = File.ReadAllText($"{time}/{inner}.json");
                    var resultI = this.env.Values.GlobalResults[time][$"{inner}"];
                    //var resultI = JsonSerializer.Deserialize<Result>(jsonI);

                    result.Add(resultI);
                }
            }
        }
        else
        {
            var jsonR = File.ReadAllText($"MapData/{regionId}.json");
            var region = JsonSerializer.Deserialize<Region>(jsonR);

            if (region.Inner != null && region.Inner.Count != 0)
            {
                foreach (var inner in region.Inner)
                {
                    //var jsonI = File.ReadAllText($"{time}/{inner}.json");
                    var resultI = this.env.Values.GlobalResults[time][$"{inner}"];
                    //var resultI = JsonSerializer.Deserialize<Result>(jsonI);

                    result.Add(resultI);
                }
            }
        }
        return result;
    }
}
