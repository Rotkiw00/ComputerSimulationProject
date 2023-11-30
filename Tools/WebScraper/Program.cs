using System.Text.Json;

(bool exit, string path, uint delay) = ArgsManager.Check(args);

if (exit) return;

var webScraper = new WebScraper.WebScraper(delay);

List<Region> regions = await webScraper.RunAsync();

string fileName = "RegionsBorderData.json";
string json = JsonSerializer.Serialize(regions);
File.WriteAllText(Path.Combine(path, fileName), json);

Console.WriteLine($"[SYSTEM] Regions' border data saved in {Path.Combine(path, fileName)}");