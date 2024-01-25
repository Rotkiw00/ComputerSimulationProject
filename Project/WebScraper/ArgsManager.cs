namespace WebScraper;

public static class ArgsManager
{
    public static (bool exit, string path, uint delay) Check(string[] args)
    {
        if (args.Contains("-h") || args.Contains("--help"))
        {
            Console.WriteLine("WebScraper tool v2.0\n");
            Console.WriteLine("-h / --help              show basic information");
            Console.WriteLine("-f / --file <PATH>       output file path (directory must exists)");
            Console.WriteLine("-d / --delay <VALUE>     delay between HTTP requests (in milliseconds)");
            return (true, "", 0);
        }

        string path = Path.GetFullPath(".");

        if (args.Contains("-f") || args.Contains("--file"))
        {
            var index = args
                .Select((arg, index) => (arg, index))
                .Where((x) => x.arg.Equals("-f") || x.arg.Equals("--file"))
                .FirstOrDefault().index;

            Console.WriteLine(args.Select((arg, index) => (arg, index)));

            if (index < args.Length - 1 && Path.Exists(args[index + 1]))
                path = args[index + 1];
        }

        uint delay = 0;

        if (args.Contains("-d") || args.Contains("--delay"))
        {
            var index = args
            .Select((arg, index) => (arg, index))
            .Where((x) => x.arg.Equals("-d") || x.arg.Equals("--delay"))
            .FirstOrDefault().index;

            if (index < args.Length - 1)
                uint.TryParse(args[index + 1], out delay);
        }

        return (false, path, delay);
    }
}
