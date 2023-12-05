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
            return Election switch
            {
                ElectionType.Sejm => DistrictId switch
                {
                    1 or 2 or 3 => "dolnośląskie",
                    4 or 5 => "kujawsko-pomorskie",
                    6 or 7 => "lubelskie",
                    8 => "lubuskie",
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
                },
                ElectionType.Senat => DistrictId switch
                {
                    1 or 2 or 3 or 4 or 5 or 6 or 7 or 8 => "dolnośląskie",
                    9 or 10 or 11 or 12 or 13 => "kujawsko-pomorskie",
                    14 or 15 or 16 or 17 or 18 or 19 => "lubelskie",
                    20 or 21 or 22 => "lubuskie",
                    23 or 24 or 25 or 26 or 27 or 28 or 29 => "łódzkie",
                    30 or 31 or 32 or 33 or 34 or 35 or 36 or 37 => "małopolskie",
                    38 or 39 or 40 or 41 or 42 or 43 or 44 or 45 or 46 or 47 or 48 or 49 or 50 => "mazowieckie",
                    51 or 52 or 53 => "opolskie",
                    54 or 55 or 56 or 57 or 58 => "podkarpackie",
                    59 or 60 or 61 => "podlaskie",
                    62 or 63 or 64 or 65 or 66 or 67 => "pomorskie",
                    68 or 69 or 70 or 71 or 72 or 73 or 74 or 75 or 76 or 77 or 78 or 79 or 80 => "śląskie",
                    81 or 82 or 83 => "świętokrzyskie",
                    84 or 85 or 86 or 87 => "warmińsko-mazurskie",
                    88 or 89 or 90 or 91 or 92 or 93 or 94 or 95 or 96 => "wielkopolskie",
                    97 or 98 or 99 or 100 => "zachodniopomorskie",
                    _ => ""
                },
                _ => ""
            };
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RegionType Type { get; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ElectionType Election { get; }

    public Region(string name, RegionType type, int districtId, ElectionType election)
    {
        Name = name;
        Type = type;
        DistrictId = districtId;
        Election = election;
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
