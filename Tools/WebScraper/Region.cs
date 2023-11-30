using System.Text.Json.Serialization;

namespace WebScraper;

public class Region
{
    public string Name { get; }
    public ulong OsmId { set; get; }
    public List<double[]>? Borders { get; set; } = null;
    public List<Region>? Inner { get; set; } = null;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RegionType Type { get; }

    public Region(string name, RegionType type)
    {
        Name = name;
        Type = type;
    }
}
