namespace ElectionSimulatorLibrary;

public class Region
{
    public string Name { get; set; }
    public int RegionId { get; set; }
    public long OsmId { set; get; }
    public bool Inhabited { get; set; } = false;
    public List<List<double[]>>? Borders { get; set; } = null;
    public List<int>? Inner { get; set; } = null;
    public int SenatDistrictId { get; set; }
    public int SejmDistrictId
    {
        get
        {
            return SenatDistrictId switch
            {
                1 or 2 or 3 => 1,
                4 or 5 => 2,
                6 or 7 or 8 => 3,
                9 or 10 => 4,
                11 or 12 or 13 => 5,
                14 or 15 or 16 => 6,
                17 or 18 or 19 => 7,
                20 or 21 or 22 => 8,
                23 or 24 => 9,
                28 or 29 => 10,
                25 or 26 or 27 => 11,
                30 => 12,
                31 or 32 or 33 => 13,
                36 or 37 => 14,
                34 or 35 => 15,
                38 or 39 => 16,
                49 or 50 => 17,
                46 or 47 or 48 => 18,
                42 or 43 or 44 or 45 => 19,
                40 or 41 => 20,
                51 or 52 or 53 => 21,
                57 or 58 => 22,
                54 or 55 or 56 => 23,
                59 or 60 or 61 => 24,
                65 or 66 or 67 => 25,
                62 or 63 or 64 => 26,
                78 or 79 => 27,
                68 or 69 => 28,
                70 or 71 => 29,
                72 or 73 => 30,
                74 or 75 or 80 => 31,
                76 or 77 => 32,
                81 or 82 or 83 => 33,
                84 or 85 => 34,
                86 or 87 => 35,
                94 or 95 or 96 => 36,
                92 or 93 => 37,
                88 or 89 => 38,
                90 or 91 => 39,
                99 or 100 => 40,
                97 or 98 => 41,
                _ => 0
            }; ;
        }
    }
    public string Voivodeship
    {
        get
        {
            return SenatDistrictId switch
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
            };
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RegionType Type { get; set; }

    public ElectionType ElectionType { set; get; } = ElectionType.Senat;

    public Region() { }
    public Region(int regionId, string name, RegionType type, int senatId)
    {
        RegionId = regionId;
        Name = name;
        Type = type;
        SenatDistrictId = senatId;
    }

    public Region(int regionId, string name, RegionType type, int senatId, long osmId, bool inhabited, List<List<double[]>>? borders, List<int> inner)
    {
        RegionId = regionId;
        Name = name;
        Type = type;
        SenatDistrictId = senatId;
        OsmId = osmId;
        Inhabited = inhabited;
        Borders = borders;
        Inner = inner;
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

    public void SaveAsFile()
    {
        if (!Directory.Exists("MapData"))
        {
            Directory.CreateDirectory("MapData");
        }

        string json = JsonSerializer.Serialize(this);
        File.WriteAllText(Path.Combine("MapData", $"{RegionId}.json"), json);
    }

    public string GetCSharpDefinition()
    {
        string result = $"new {this.GetType().Name}({RegionId}, \"{Name}\", {this.Type.GetType().Name}.{Type.ToString("G")}, {SenatDistrictId}, {OsmId}, {Inhabited}, {{0}}, {{1}});";

        string bordersResult = "";
        if (Borders != null && Borders.Count != 0)
        {
            bordersResult = "[";
            foreach (var polygon in Borders)
            {
                bordersResult += "[";
                foreach (var points in polygon)
                {
                    bordersResult += $"[{points[0]},{points[1]}],";
                }
                bordersResult = bordersResult.Substring(0, bordersResult.Length - 1);
                bordersResult += "],";
            }
            bordersResult = bordersResult.Substring(0, bordersResult.Length - 1);
            bordersResult += "]";
        }
        else bordersResult = "null";

        string innerResult = "";
        if (Inner != null && Inner.Count != 0)
        {
            innerResult = "[";
            foreach (var innerId in Inner)
            {
                innerResult += $"{innerId},";
            }
            innerResult = innerResult.Substring(0, innerResult.Length - 1);
            innerResult += "]";
        }
        else innerResult = "null";
        
        result = String.Format(result, bordersResult, innerResult);
        return result;
    }
}
