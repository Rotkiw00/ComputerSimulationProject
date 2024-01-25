(bool exit, string path, uint delay) = ArgsManager.Check(args);

if (exit) return;

var webScraper = new WebScraper.WebScraper(delay);

await webScraper.RunAsync();

string fileName = "RegionsData";
string dataPath = "MapData";
ZipFile.CreateFromDirectory(dataPath, $"{fileName}.zip");
if (File.Exists(Path.Combine(path, $"{fileName}.map")))
    File.Delete(Path.Combine(path, $"{fileName}.map"));
File.Move($"{fileName}.zip", Path.Combine(path, $"{fileName}.map"));
Directory.Delete(dataPath, true);

Console.WriteLine($"[SYSTEM] Regions' border data saved in {Path.Combine(path, $"{fileName}.map")}");