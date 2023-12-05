using System.Text.Json;

(bool exit, string path, uint delay) = ArgsManager.Check(args);

if (exit) return;

var webScraper = new WebScraper.WebScraper(delay);

var result = await webScraper.RunAsync();

string fileName = "RegionsBorderData.json";
string json = JsonSerializer.Serialize(result);
File.WriteAllText(Path.Combine(path, fileName), json);

Console.WriteLine($"[SYSTEM] Regions' border data saved in {Path.Combine(path, fileName)}");