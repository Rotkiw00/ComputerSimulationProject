using System.Drawing;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = System.Drawing.Color;

namespace ElectionSimulatorLibrary.WPF;

public class SimMap : Canvas
{
    public class Creator
    {
        byte _colorR = 0;
        byte _colorG = 0;
        byte _colorB = 0;

        public double Size { get; set; } = 100;
        public ElectionType ElectionType { get; set; } = ElectionType.Senat;
        public MapMode MapMode { get; set; } = MapMode.Normal;
        public int RegionId { get; set; } = 0;
        public byte ColorR { get => _colorR; set { this.Color = Color.FromArgb(value, _colorG, _colorB); } }
        public byte ColorG { get => _colorG; set { this.Color = Color.FromArgb(_colorR, value, _colorB); } }
        public byte ColorB { get => _colorB; set { this.Color = Color.FromArgb(_colorR, _colorB, value); } }
        public Color Color { get; set; } = Color.Black;
        public RegionClicked OnClick { get; set; } = (int id) => { };
        public double StrokeThickness { get; set; } = 1;

        // TODO result mode

        public SimMap? Create()
        {
            if (RegionHasInner(this.RegionId))
                return new SimMap(this);
            else
                return null;
        }
    }

    private double size;
    private ElectionType electionType;
    private MapMode mapMode;
    private int regionId;
    private Color color = Color.Black;
    private RegionClicked onClick;
    private double strokeThickness;

    private List<RegionButton> navButtons;
    private List<RegionShape> shapes;

    // TODO timer
    private SimMap(Creator creator)
    {
        this.size = creator.Size;
        this.electionType = creator.ElectionType;
        this.mapMode = creator.MapMode;
        this.regionId = creator.RegionId;
        this.color = creator.Color;
        this.onClick = creator.OnClick;
        this.strokeThickness = creator.StrokeThickness;

        // TODO if (!Directory.Exists("MapData")) { }

        this.Width = this.size;
        this.Height = this.size;

        List<Region> regionList = GetRegionList();
        var space = EstimateSpace(regionList);
        DrawMap(regionList, space.scale, space.spaceX, space.spaceY);
    }

    public delegate void RegionClicked(int id);

    private List<Region> GetRegionList()
    {
        List<Region> regionList;

        if (this.regionId == 0) // Whole country
        {
            var senatList = new List<Region>();

            for (int i = 1; i <= 100; i++)
            {
                var json = File.ReadAllText($"MapData/{i}.json");
                var region = JsonSerializer.Deserialize<Region>(json);
                senatList.Add(region!);
            }

            if (electionType == ElectionType.Senat)
            {
                regionList = senatList;
            }
            else
            {
                regionList = new List<Region>();

                for (int i = 1; i <= 41; i++)
                {
                    Region region = new Region();
                    region.Name = $"Okręg {i}";
                    region.RegionId = i;
                    region.Inner = new();
                    region.Type = RegionType.ElectoralDistrict;
                    region.ElectionType = ElectionType.Sejm;

                    var sejmRegionElements = senatList
                        .Where(r => r.SejmDistrictId == i)
                        .ToList();

                    foreach (var element in sejmRegionElements)
                    {
                        element.Inner!.ForEach((id) => { region.Inner.Add(id); });
                    }

                    regionList.Add(region);
                }
            }

            foreach (var region in regionList)
            {
                region.Borders = new();
                foreach (var subRegionId in region.Inner!)
                {
                    var json = File.ReadAllText($"MapData/{subRegionId}.json");
                    var subRegion = JsonSerializer.Deserialize<Region>(json);

                    foreach (var polygon in subRegion!.Borders!)
                    {
                        region.Borders.Add(polygon);
                    }
                }
            }
        }
        else // Region elements
        {
            regionList = new();

            List<int> subRegionIds = new();

            if (electionType == ElectionType.Sejm && regionId <= 41)
            {
                subRegionIds = new();

                List<Region> senatList = new();
                for (int i = 1; i <= 100; i++)
                {
                    var jsonSenat = File.ReadAllText($"MapData/{i}.json");
                    var regionSenat = JsonSerializer.Deserialize<Region>(jsonSenat);
                    senatList.Add(regionSenat!);
                }

                var sejmRegionElements = senatList
                    .Where(r => r.SejmDistrictId == regionId)
                    .ToList();

                foreach (var element in sejmRegionElements)
                {
                    element.Inner!.ForEach((id) => { subRegionIds.Add(id); });
                }

            }
            else
            {
                var json = File.ReadAllText($"MapData/{regionId}.json");
                var region = JsonSerializer.Deserialize<Region>(json);

                subRegionIds = region!.Inner!;
            }

            foreach (var subRegionId in subRegionIds)
            {
                var json = File.ReadAllText($"MapData/{subRegionId}.json");
                var region = JsonSerializer.Deserialize<Region>(json);

                region!.ElectionType = this.electionType;
                regionList.Add(region);
            }
        }

        return regionList;
    }

    private (double scale, double spaceX, double spaceY) EstimateSpace(List<Region> regionList)
    {
        double min_x = 9999;
        double min_y = 9999;
        double max_x = 0;
        double max_y = 0;

        foreach (var region in regionList)
        {
            foreach (var polygon in region!.Borders!)
            {
                foreach (var coord in polygon)
                {
                    if (coord[0] < min_x) min_x = coord[0];
                    if (coord[1] < min_y) min_y = coord[1];
                    if (coord[0] > max_x) max_x = coord[0];
                    if (coord[1] > max_y) max_y = coord[1];
                }
            }
        }

        double range_x = max_x - min_x;
        double range_y = max_y - min_y;

        double original_x = regionList[0].Borders![0][0][0];
        double test_x = ((original_x - min_x) / range_x) * size;
        double scale_x = test_x / original_x;

        double original_y = regionList[0].Borders![0][0][1];
        double test_y = ((original_y - min_y) / range_y) * size;
        double scale_y = test_y / original_y;

        double scale = scale_x < scale_y ? scale_x : scale_y;

        double new_max_x = scale * max_x;
        double new_max_y = scale * max_y;

        double spaceX = (size - new_max_x) / 2;
        double spaceY = (size - new_max_y) / 2;

        return (scale, spaceX, spaceY);
    }

    private void DrawMap(List<Region> regionList, double scale, double spaceX, double spaceY)
    {
        navButtons = new();
        shapes = new();

        foreach (var region in regionList)
        {
            RegionShape shape = new RegionShape();
            shape.Region = region;
            shape.OnClick = onClick;
            shape.MapMode = mapMode;
            shape.Color = color;
            shape.StrokeThickness = strokeThickness;

            foreach (var polygonData in region!.Borders!)
            {
                PointCollection points = new();

                foreach (var coord in polygonData)
                {
                    double x = (coord[0] * scale) + spaceX;
                    double y = (coord[1] * scale) + spaceY;
                    points.Add(new(x, size - y));
                }

                var polygon = new Polygon();
                polygon.Points = points;

                shape.Polygons!.Add(polygon);
            }

            shape.Setup();
            RegionButton rBtn = new(shape);

            shapes.Add(shape);
            navButtons.Add(rBtn);
        }

        foreach (var shape in shapes)
        {
            foreach (var polygon in shape.Polygons)
            {
                this.Children.Add(polygon);
            }
        }
    }

    public (SimMap, List<RegionButton>) GetMap()
    {
        return (this, navButtons);
    }

    public static bool RegionHasInner(int id)
    {
        if (id == 0)
        {
            for (int i = 1; i <= 100; i++)
            {
                if (!File.Exists($"MapData/{i}.json"))
                    return false;
                return true;
            }
        }
        try
        {
            var json = File.ReadAllText($"MapData/{id}.json");
            var region = JsonSerializer.Deserialize<Region>(json);

            return region?.Inner != null && region.Inner.Count != 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public static bool LoadDataFromFile(string mapFilePath)
    {
        try
        {
            if (Directory.Exists("MapData")) Directory.Delete("MapData", true);
            File.Copy(mapFilePath, "tmp.zip", true);
            string extractPath = "MapData";
            ZipFile.ExtractToDirectory("tmp.zip", extractPath);
            File.Delete("tmp.zip");

            bool dataExists = false;

            if (Directory.Exists("MapData"))
            {
                dataExists = true;
                for (int i = 1; i <= 100; i++)
                {
                    if (!File.Exists($"MapData/{i}.json"))
                        dataExists = false;
                }
            }

            return dataExists;
        }
        catch
        {
            return false;
        }
    }
}