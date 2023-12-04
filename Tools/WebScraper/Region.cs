using System.Text.Json.Serialization;

namespace WebScraper;

public class Region
{
    public string Name { get; private set; }
    public ulong OsmId { set; get; }
    public List<double[]>? Borders { get; set; } = null;
    public List<Region>? Inner { get; set; } = null;
    public int DistrictId { get; }
    public string Voivodeship
    {
        get
        {
            return DistrictId switch
            {
                1 or 2 or 3 => "dolnośląskie",
                4 or 5 => "kujawsko-pomorskie",
                6 or 7 or 8 => "lubelskie",
                9 or 10 or 11 => "łódzkie",
                12 or 13 or 14 or 15 => "małopolskie",
                16 or 17 or 18 or 19 or 20 => "mazowieckie",
                21 => "opolskie",
                22 or 23 => "podkarpackie",
                24 => "podlaskie",
                25 or 26 => "pomorskie",
                27 or 28 or 29 or 30 or 31 or 32 => "śląskie",
                33 => "świętokrzyskie",
                34 or 35 => "warmińsko-mazurskie",
                36 or 37 or 38 or 39 => "wielkopolskie",
                40 or 41 => "zachodniopomorskie",
                _ => ""
            };
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RegionType Type { get; }

    public Region(string name, RegionType type, int districtId)
    {
        Name = name;
        Type = type;
        DistrictId = districtId;
    }

    public void Fix()
    {
        Name = Name switch
        {
            "Szczawin Kośc." => "Szczawin Kościelny",
            "Słupia (Jędrzejowska)" => "Słupia",
            _ => Name
        };
    }
}
