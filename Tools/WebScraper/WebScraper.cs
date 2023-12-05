namespace WebScraper;

public class WebScraper
{
    private uint _delay;

    public WebScraper(uint delay = 0)
    {
        _delay = delay;
    }

    public async Task<Dictionary<string, List<Region>>> RunAsync()
    {
        var result = new Dictionary<string, List<Region>>();
        var sejm = new List<Region>();
        var senat = new List<Region>();

        LoadPkwSejm(sejm);
        await LoadOsmIdAsync(sejm);
        await LoadOsmBordersAsync(sejm);

        LoadPkwSenat(senat);
        await LoadOsmIdAsync(senat);
        await LoadOsmBordersAsync(senat);

        result.Add("sejm", sejm);
        result.Add("senat", senat);
        return result;
    }

    public void LoadPkwSejm(List<Region> regions)
    {
        Console.WriteLine("[SYSTEM] Load PKW data (sejm)");

        IWebDriver driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(_delay);

        for (int i = 1; i <= 41; i++)
        {
            Console.WriteLine($"[INFO] Okręg {i}");

            Region currentRegion = new($"Okręg {i}", RegionType.ElectoralDistrict, i, ElectionType.Sejm);
            currentRegion.Inner = new();

            if (i == 19)
            {
                Console.WriteLine($"[INFO] Okręg {i}, Warszawa");

                Region inner = new("Warszawa", RegionType.CityWithCountyRights, i, ElectionType.Sejm);
                currentRegion.Inner.Add(inner);
                regions.Add(currentRegion);
                continue;
            }

            string url = $"https://wybory.gov.pl/sejmsenat2023/pl/sejm/wynik/okr/{i}";
            Thread.Sleep((int)_delay);
            driver.Navigate().GoToUrl(url);

            var htmlResult = driver.FindElement(By.CssSelector("div#root"));
            htmlResult = htmlResult.FindElement(By.CssSelector("div.res"));
            htmlResult = htmlResult.FindElement(By.CssSelector("div.row"));
            htmlResult = htmlResult.FindElement(By.CssSelector("div.stats.col-xs-12.col-md-6.col-lg-7"));
            htmlResult = htmlResult.FindElement(By.CssSelector("div.s"));
            htmlResult = htmlResult.FindElement(By.CssSelector("ul.list"));
            htmlResult = htmlResult.FindElements(By.TagName("ul"))
                .Select((data, index) => (data, index))
                .OrderByDescending(x => x.index)
                .First().data;
            var elementsOfCurrentRegion = htmlResult.FindElements(By.TagName("li"))
                .Select((data, index) =>
                {
                    string text = data.Text;
                    string url = data
                        .FindElement(By.TagName("a"))
                        .GetAttribute("href");
                    return (text, url);
                }).ToArray();

            foreach (var currentRegionElement in elementsOfCurrentRegion)
            {
                string elementName = currentRegionElement.text;
                Console.WriteLine($"[INFO] Okręg {i}, {elementName}");

                Region r0;

                if (elementName.StartsWith("Powiat "))
                {
                    r0 = new Region(elementName, RegionType.County, i, ElectionType.Sejm);
                    r0.Inner = new();

                    var urlCounty = currentRegionElement.url;
                    Thread.Sleep((int)_delay);
                    driver.Navigate().GoToUrl(urlCounty);

                    var htmlResultInner = driver.FindElement(By.CssSelector("div#root"));
                    htmlResultInner = htmlResultInner.FindElement(By.CssSelector("div.res"));
                    htmlResultInner = htmlResultInner.FindElement(By.CssSelector("div.row"));
                    htmlResultInner = htmlResultInner.FindElement(By.CssSelector("div.stats.col-xs-12.col-md-6.col-lg-7"));
                    htmlResultInner = htmlResultInner.FindElement(By.CssSelector("div.s"));
                    htmlResultInner = htmlResultInner.FindElement(By.CssSelector("ul.list"));
                    htmlResultInner = htmlResultInner.FindElements(By.TagName("ul"))
                        .Select((data, index) => (data, index))
                        .OrderByDescending(x => x.index)
                        .First().data;
                    var elementsOfCurrentCounty = htmlResultInner.FindElements(By.TagName("li"))
                        .Select((data, index) =>
                        {
                            string text = data.Text;
                            string url = data
                                .FindElement(By.TagName("a"))
                                .GetAttribute("href");
                            return (text, url);
                        }).ToArray();

                    foreach (var currentCountyElement in elementsOfCurrentCounty)
                    {
                        string countyElementName = currentCountyElement.text;
                        Console.WriteLine($"[INFO] Okręg {i}, {elementName}, {countyElementName}");

                        Region r1;
                        if (countyElementName.StartsWith("gm. "))
                        {
                            countyElementName = countyElementName.Replace("gm. ", "");
                            r1 = new Region(countyElementName, RegionType.Municipality, i, ElectionType.Sejm);
                        }
                        else
                        {
                            countyElementName = countyElementName.Replace("m. ", "");
                            r1 = new Region(countyElementName, RegionType.City, i, ElectionType.Sejm);
                        }
                        r0.Inner.Add(r1);
                    }
                }
                else
                {
                    elementName = elementName.Replace("Miasto na prawach powiatu ", "");
                    r0 = new Region(elementName, RegionType.CityWithCountyRights, i, ElectionType.Sejm);
                }

                currentRegion.Inner.Add(r0);
            }

            regions.Add(currentRegion);
        }
        driver.Close();

        foreach (var region in regions)
        {
            FixRecursively(region);
        }
    }
    public void LoadPkwSenat(List<Region> regions)
    {
        Console.WriteLine("[SYSTEM] Load PKW data (senat)");

        IWebDriver driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(_delay);

        for (int i = 1; i <= 100; i++)
        {
            Console.WriteLine($"[INFO] Okręg {i}");

            Region currentRegion = new($"Okręg {i}", RegionType.ElectoralDistrict, i, ElectionType.Senat);
            currentRegion.Inner = new();

            List<int> exceptions = new() { 7, 8, 23, 24, 32, 33, 42, 43, 44, 45 };

            if (exceptions.Contains(i))
            {
                var list = currentRegion.Inner;
                Console.WriteLine($"[INFO] Okręg {i}");
                switch (i)
                {
                    case 7:
                        string[] districts7 = { "Wrocław, Osiedle Bieńkowice", "Wrocław, Osiedle Biskupin-Sępolno-Dąbie-Bartoszowice", "Wrocław, Osiedle Borek", "Wrocław, Osiedle Brochów", "Wrocław, Osiedle Gaj", "Wrocław, Osiedle Gajowice", "Wrocław, Osiedle Grabiszyn-Grabiszynek", "Wrocław, Osiedle Huby", "Wrocław, Osiedle Jagodno", "Wrocław, Osiedle Klecina", "Wrocław, Osiedle Krzyki-Partynice", "Wrocław, Osiedle Księże", "Wrocław, Osiedle Oporów", "Wrocław, Osiedle Plac Grunwaldzki", "Wrocław, Osiedle Powstańców Śląskich", "Wrocław, Osiedle Przedmieście Oławskie", "Wrocław, Osiedle Przedmieście Świdnickie", "Wrocław, Osiedle Stare Miasto", "Wrocław, Osiedle Tarnogaj", "Wrocław, Osiedle Wojszyce", "Wrocław, Osiedle Ołtaszyn", "Wrocław, Osiedle Zacisze-Zalesie-Szczytniki" };
                        foreach(var district in districts7)
                        {
                            list.Add(new(district, RegionType.CityDistrict, i, ElectionType.Senat));
                            Console.WriteLine($"[INFO] Okręg {i}, {district}");
                        }
                        break;


                    case 8:
                        string[] districts8 = { "Wrocław, Osiedle Gądów-Popowice Południowe", "Wrocław, Osiedle Jerzmanowo-Jarnołtów-Strachowice-Osiniec", "Wrocław, Osiedle Karłowice-Różanka", "Wrocław, Osiedle Kleczków", "Wrocław, Osiedle Kowale", "Wrocław, Osiedle Kużniki", "Wrocław, Osiedle Leśnica", "Wrocław, Osiedle Lipa Piotrowska", "Wrocław, Osiedle Maślice", "Wrocław, Osiedle Muchobór Mały", "Wrocław, Osiedle Muchobór Wielki", "Wrocław, Osiedle Nadodrze", "Wrocław, Osiedle Nowy Dwór", "Wrocław, Osiedle Ołbin", "Wrocław, Osiedle Osobowice-Rędzin", "Wrocław, Osiedle Pawłowice", "Wrocław, Osiedle Pilczyce-Kozanów-Popowice", "Wrocław, Osiedle Polanowice-Poświętne-Ligota", "Wrocław, Osiedle Pracze Odrzańskie", "Wrocław, Osiedle Psie Pole-Zawidawie", "Wrocław, Osiedle Sołtysowice", "Wrocław, Osiedle Swojczyce-Strachocin-Wojnów", "Wrocław, Osiedle Szczepin", "Wrocław, Osiedle Świniary", "Wrocław, Osiedle Widawa", "Wrocław, Osiedle Żerniki" };
                        foreach (var district in districts8)
                        {
                            list.Add(new(district, RegionType.CityDistrict, i, ElectionType.Senat));
                            Console.WriteLine($"[INFO] Okręg {i}, {district}");
                        }
                        break;


                    case 23:
                        string[] districts23 = { "Łódź, Bałuty Zachodnie", "Łódź, Bałuty-Centrum", "Łódź, Bałuty-Doły", "Łódź, im Józefa Montwiłła-Mireckiego", "Łódź, Julianów-Marysin-Rogi", "Łódź, Karolew-Retkinia Wschód", "Łódź, Katedralna", "Łódź, Koziny", "Łódź, Lublinek-Pienista", "Łódź, Łagiewniki", "Łódź, Nad Nerem", "Łódź, Radogoszcz", "Łódź, Retkinia Zachód-Smulsko", "Łódź, Stare Polesie", "Łódź, Śródmieście-Wschód", "Łódź, Teofilów-Wielkopolska", "Łódź, Zdrowie-Mania", "Łódź, Złotno" };
                        foreach (var district in districts23)
                        {
                            list.Add(new(district, RegionType.CityDistrict, i, ElectionType.Senat));
                            Console.WriteLine($"[INFO] Okręg {i}, {district}");
                        }
                        break;


                    case 24:
                        Console.WriteLine($"[INFO] Okręg {i}, powiat łódzki wschodni");
                        Region county1 = new("Powiat łódzki wschodni", RegionType.County, i, ElectionType.Senat);
                        county1.Inner = new();
                        string[] municipalities1 = { "Andrespol", "Brójce", "Koluszki", "Nowosolna", "Rzgów", "Tuszyn" };
                        foreach(var municipality in municipalities1)
                        {
                            Console.WriteLine($"[INFO] Okręg {i}, powiat łódzki wschodni, gm. {municipality}");
                            county1.Inner.Add(new(municipality, RegionType.Municipality, i, ElectionType.Senat));
                        }
                        list.Add(county1);

                        Console.WriteLine($"[INFO] Okręg {i}, powiat brzeziński");
                        Region county2 = new("Powiat brzeziński", RegionType.County, i, ElectionType.Senat);
                        county2.Inner = new();
                        string[] municipalities2 = { "Brzeziny", "Dmosin", "Jeżów", "Rogów" };
                        foreach (var municipality in municipalities2)
                        {
                            Console.WriteLine($"[INFO] Okręg {i}, powiat brzeziński, gm. {municipality}");
                            county2.Inner.Add(new(municipality, RegionType.Municipality, i, ElectionType.Senat));
                        }
                        county2.Inner.Add(new("Brzeziny", RegionType.City, i, ElectionType.Senat));
                        Console.WriteLine($"[INFO] Okręg {i}, powiat brzeziński, m. Brzeziny");
                        list.Add(county2);

                        string[] districts24 = { "Łódź, Andrzejów", "Łódź, Chojny", "Łódź, Chojny-Dąbrowa", "Łódź, Dolina Łódki", "Łódź, Górniak", "Łódź, Mileszki", "Łódź, Nowosolna", "Łódź, Nr 33", "Łódź, Olechów-Janów", "Łódź, Piastów-Kurak", "Łódź, Rokicie", "Łódź, Ruda", "Łódź, Stary Widzew", "Łódź, Stoki-Sikawa-Podgórze", "Łódź, Widzew-Wschód", "Łódź, Wiskitno", "Łódź, Wzniesień Łódzkich", "Łódź, Zarzew" };
                        foreach (var district in districts24)
                        {
                            list.Add(new(district, RegionType.CityDistrict, i, ElectionType.Senat));
                            Console.WriteLine($"[INFO] Okręg {i}, {district}");
                        }
                        break;


                    case 32:
                        string[] districts32 = { "Kraków, dzielnica II", "Kraków, dzielnica III", "Kraków, dzielnica IV", "Kraków, dzielnica XIV", "Kraków, dzielnica XV", "Kraków, dzielnica XVI", "Kraków, dzielnica XVII", "Kraków, dzielnica XVIII" };
                        foreach (var district in districts32)
                        {
                            list.Add(new(district, RegionType.CityDistrict, i, ElectionType.Senat));
                            Console.WriteLine($"[INFO] Okręg {i}, {district}");
                        }
                        break;


                    case 33:
                        string[] districts33 = { "Kraków, dzielnica I", "Kraków, dzielnica V", "Kraków, dzielnica VI", "Kraków, dzielnica VII", "Kraków, dzielnica VIII", "Kraków, dzielnica IX", "Kraków, dzielnica X", "Kraków, dzielnica XI", "Kraków, dzielnica XII", "Kraków, dzielnica XIII" };
                        foreach (var district in districts33)
                        {
                            list.Add(new(district, RegionType.CityDistrict, i, ElectionType.Senat));
                            Console.WriteLine($"[INFO] Okręg {i}, {district}");
                        }
                        break;
                    case 42:
                        string[] districts42 = { "Warszawa, Praga-Południe", "Warszawa, Praga-Północ", "Warszawa, Rembertów", "Warszawa, Targówek", "Warszawa, Wesoła" };
                        foreach (var district in districts42)
                        {
                            list.Add(new(district, RegionType.CityDistrict, i, ElectionType.Senat));
                            Console.WriteLine($"[INFO] Okręg {i}, {district}");
                        }
                        break;
                    case 43:
                        string[] districts43 = { "Warszawa, Mokotów", "Warszawa, Ursynów", "Warszawa, Wawer", "Warszawa, Wilanów" };
                        foreach (var district in districts43)
                        {
                            list.Add(new(district, RegionType.CityDistrict, i, ElectionType.Senat));
                            Console.WriteLine($"[INFO] Okręg {i}, {district}");
                        }
                        break;
                    case 44:
                        string[] districts44 = { "Warszawa, Białołęka", "Warszawa, Bielany", "Warszawa, Śródmieście", "Warszawa, Żoliborz" };
                        foreach (var district in districts44)
                        {
                            list.Add(new(district, RegionType.CityDistrict, i, ElectionType.Senat));
                            Console.WriteLine($"[INFO] Okręg {i}, {district}");
                        }
                        break;
                    case 45:
                        string[] districts45 = { "Warszawa, Bemowo", "Warszawa, Ochota", "Warszawa, Ursus", "Warszawa, Włochy", "Warszawa, Wola" };
                        foreach (var district in districts45)
                        {
                            list.Add(new(district, RegionType.CityDistrict, i, ElectionType.Senat));
                            Console.WriteLine($"[INFO] Okręg {i}, {district}");
                        }
                        break;
                }
                regions.Add(currentRegion);
                continue;
            }

            string url = $"https://wybory.gov.pl/sejmsenat2023/pl/senat/wynik/okr/{i}";
            Thread.Sleep((int)_delay);
            driver.Navigate().GoToUrl(url);

            var htmlResult = driver.FindElement(By.CssSelector("div#root"));
            htmlResult = htmlResult.FindElement(By.CssSelector("div.res"));
            htmlResult = htmlResult.FindElement(By.CssSelector("div.row"));
            htmlResult = htmlResult.FindElement(By.CssSelector("div.stats.col-xs-12.col-md-6.col-lg-7"));
            htmlResult = htmlResult.FindElement(By.CssSelector("div.s"));
            htmlResult = htmlResult.FindElement(By.CssSelector("ul.list"));
            htmlResult = htmlResult.FindElements(By.TagName("ul"))
                .Select((data, index) => (data, index))
                .OrderByDescending(x => x.index)
                .First().data;
            var elementsOfCurrentRegion = htmlResult.FindElements(By.TagName("li"))
                .Select((data, index) =>
                {
                    string text = data.Text;
                    string url = data
                        .FindElement(By.TagName("a"))
                        .GetAttribute("href");
                    return (text, url);
                }).ToArray();

            foreach (var currentRegionElement in elementsOfCurrentRegion)
            {
                string elementName = currentRegionElement.text;
                Console.WriteLine($"[INFO] Okręg {i}, {elementName}");

                Region r0;

                if (elementName.StartsWith("Powiat "))
                {
                    r0 = new Region(elementName, RegionType.County, i, ElectionType.Senat);
                    r0.Inner = new();

                    var urlCounty = currentRegionElement.url;
                    Thread.Sleep((int)_delay);
                    driver.Navigate().GoToUrl(urlCounty);

                    var htmlResultInner = driver.FindElement(By.CssSelector("div#root"));
                    htmlResultInner = htmlResultInner.FindElement(By.CssSelector("div.res"));
                    htmlResultInner = htmlResultInner.FindElement(By.CssSelector("div.row"));
                    htmlResultInner = htmlResultInner.FindElement(By.CssSelector("div.stats.col-xs-12.col-md-6.col-lg-7"));
                    htmlResultInner = htmlResultInner.FindElement(By.CssSelector("div.s"));
                    htmlResultInner = htmlResultInner.FindElement(By.CssSelector("ul.list"));
                    htmlResultInner = htmlResultInner.FindElements(By.TagName("ul"))
                        .Select((data, index) => (data, index))
                        .OrderByDescending(x => x.index)
                        .First().data;
                    var elementsOfCurrentCounty = htmlResultInner.FindElements(By.TagName("li"))
                        .Select((data, index) =>
                        {
                            string text = data.Text;
                            string url = data
                                .FindElement(By.TagName("a"))
                                .GetAttribute("href");
                            return (text, url);
                        }).ToArray();

                    foreach (var currentCountyElement in elementsOfCurrentCounty)
                    {
                        string countyElementName = currentCountyElement.text;
                        Console.WriteLine($"[INFO] Okręg {i}, {elementName}, {countyElementName}");

                        Region r1;
                        if (countyElementName.StartsWith("gm. "))
                        {
                            countyElementName = countyElementName.Replace("gm. ", "");
                            r1 = new Region(countyElementName, RegionType.Municipality, i, ElectionType.Senat);
                        }
                        else
                        {
                            countyElementName = countyElementName.Replace("m. ", "");
                            r1 = new Region(countyElementName, RegionType.City, i, ElectionType.Senat);
                        }
                        r0.Inner.Add(r1);
                    }
                }
                else
                {
                    elementName = elementName.Replace("Miasto na prawach powiatu ", "");
                    r0 = new Region(elementName, RegionType.CityWithCountyRights, i, ElectionType.Senat);
                }

                currentRegion.Inner.Add(r0);
            }

            regions.Add(currentRegion);
        }
        driver.Close();

        foreach (var region in regions)
        {
            FixRecursively(region);
        }
    }
    private async Task LoadOsmIdAsync(List<Region> regions)
    {
        Console.WriteLine("[SYSTEM] Load OSM relation ids");

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", ".NET application");

        foreach (var region in regions)
        {
            await GetRegionId(client, region);
        }
    }
    private async Task LoadOsmBordersAsync(List<Region> regions)
    {
        Console.WriteLine("[SYSTEM] Load OSM borders data");

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", ".NET application");

        foreach (var region in regions)
        {
            await GetRegionBorders(client, region);
        }
    }

    private async Task GetRegionId(HttpClient client, Region region, Region? parentRegion = null)
    {
        string baseUrl = "https://nominatim.openstreetmap.org/search.php?q={0}&format=jsonv2";

        if (region.Inner != null)
            foreach (var innerRegion in region.Inner)
            {
                await GetRegionId(client, innerRegion, region);
            }

        if (region.Type == RegionType.ElectoralDistrict) return;

        string nameParam = region.Name;

        if (region.Type == RegionType.Municipality)
            nameParam = $"gmina {nameParam}";

        Console.Write($"[INFO] {nameParam}, ");

        if (parentRegion != null && parentRegion.Type == RegionType.County)
        {
            nameParam += $"%2C {parentRegion.Name}";
            Console.Write($"{parentRegion.Name.ToLower()}, ");
        }

        Console.Write($"województwo {region.Voivodeship}, ");
        if(region.Type != RegionType.CityDistrict)
            nameParam += $"%2C województwo {region.Voivodeship}";

        nameParam = nameParam.Replace(' ', '+');
        string request = String.Format(baseUrl, nameParam);

        Thread.Sleep((int)_delay);
        var jsonString = await client.GetStringAsync(request);
        var jsonObj = JsonDocument.Parse(jsonString);

        ulong? id = null;

        try
        {
            foreach (var record in jsonObj.RootElement.EnumerateArray())
            {
                string? category = record.GetProperty("category").GetString();
                string? type = record.GetProperty("type").GetString();
                if (category == "boundary" && type == "administrative")
                {
                    id = record.GetProperty("osm_id").GetUInt64();
                    break;
                }
            }

            if (id == null)
            {
                throw new Exception("Data not found.");
            }

        }
        catch
        {
            nameParam = nameParam.Replace("gmina+", "");
            request = String.Format(baseUrl, nameParam);
            Thread.Sleep((int)_delay);
            jsonString = await client.GetStringAsync(request);
            jsonObj = JsonDocument.Parse(jsonString);

            foreach (var record in jsonObj.RootElement.EnumerateArray())
            {
                string? category = record.GetProperty("category").GetString();
                string? type = record.GetProperty("type").GetString();
                if (category == "boundary" && type == "administrative")
                {
                    id = record.GetProperty("osm_id").GetUInt64();
                    break;
                }
            }

            if (id == null)
            {
                throw new Exception("Data not found.");
            }
        }

        region.OsmId = (ulong)id;

        Console.WriteLine($"id: {id}");
    }
    private async Task GetRegionBorders(HttpClient client, Region region)
    {
        string baseUrl = "https://polygons.openstreetmap.fr/get_geojson.py?id={0}&params=0";

        if (region.Inner != null)
            foreach (var innerRegion in region.Inner)
            {
                await GetRegionBorders(client, innerRegion);
            }

        if (region.Type == RegionType.ElectoralDistrict) return;

        Console.Write($"[INFO] Getting borders data for {region.Name} (id: {region.OsmId})");

        string request = String.Format(baseUrl, region.OsmId);

        Thread.Sleep((int)_delay);
        var jsonString = await client.GetStringAsync(request);
        var jsonObj = JsonDocument.Parse(jsonString);

        region.Borders = new();

        var coordinates = jsonObj.RootElement.GetProperty("coordinates")[0][0];
        foreach (var coord in coordinates.EnumerateArray())
        {
            region.Borders.Add([coord[0].GetDouble(), coord[1].GetDouble()]);
        }
        Console.WriteLine(" - Complete");
    }

    private void FixRecursively(Region region)
    {
        if (region.Inner != null)
        {
            foreach (var r in region.Inner)
            {
                FixRecursively(r);
            }
        }
        region.Fix();
    }
}
