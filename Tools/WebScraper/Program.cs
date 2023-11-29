(bool exit, string path, uint delay) = ArgsManager.Check(args);

if (exit) return;

var webScraper = new WebScraper.WebScraper(delay);

webScraper.Run();