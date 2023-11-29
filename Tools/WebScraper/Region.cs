namespace WebScraper;

public class Region
{
    public string Name { get; }
    public RegionType Type { get; }
    public ulong OsmId { set; get; }
    public List<(double x, double y)>? Borders { get; } = null;
    public List<Region>? Inner { get; set; } = null;

    public Region(string name, RegionType type)
    {
        Name = name;
        Type = type;
    }
}
