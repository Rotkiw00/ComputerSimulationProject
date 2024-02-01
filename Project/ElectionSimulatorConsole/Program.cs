using ElectionSimulatorLibrary;

DemographySettings d = new();
PoliticalSettings p = new();
p.Default();
Simulation test = new(d, p);

await test.Start();

for (int i = 0; i < 2; i++)
{
    List<Result> result = test.GetResult(0, ElectionType.Senat, i);
    Console.WriteLine($"Data: {BaseValues.TimeToDateTime(i)}");
    
    foreach (var res in result)
	{
        Console.WriteLine($"RegionId: {res.RegionId}");
        foreach(var party in res.Popularity)
        {
            Console.WriteLine($"{party.Key.Name} : {party.Value}");
        }
	}
}