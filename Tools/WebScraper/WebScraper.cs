﻿namespace WebScraper;

public class WebScraper
{
    private uint _delay;

    public WebScraper(uint delay = 0)
    {
        _delay = delay;
    }

    public List<Region> Run()
    {
        var regions = new List<Region>();
        LoadPkwSejm(regions);
        LoadOsmId(regions);
        LoadOsmBorders(regions);
        return regions;
    }

    public void LoadPkwSejm(List<Region> regions)
    {
        Console.WriteLine("[SYSTEM] Load PKW data (sejm)");

        IWebDriver driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(_delay);

        for (int i = 1; i <= 41; i++)
        {
            Console.WriteLine($"[INFO] Okręg {i}");

            Region currentRegion = new($"Okręg {i}", RegionType.ElectoralDistrict);
            currentRegion.Inner = new();

            if (i == 19)
            {
                Console.WriteLine($"[INFO] Okręg {i}, Warszawa");

                Region inner = new("Warszawa", RegionType.CityWithCountyRights);
                currentRegion.Inner.Add(inner);
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
                    r0 = new Region(elementName, RegionType.County);
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
                            r1 = new Region(countyElementName, RegionType.Municipality);
                        }
                        else
                        {
                            countyElementName = countyElementName.Replace("m. ", "");
                            r1 = new Region(countyElementName, RegionType.City);
                        }
                        r0.Inner.Add(r1);
                    }
                }
                else
                {
                    elementName = elementName.Replace("Miasto na prawach powiatu ", "");
                    r0 = new Region(elementName, RegionType.CityWithCountyRights);
                }

                currentRegion.Inner.Add(r0);
            }

            regions.Add(currentRegion);
        }
    }
    public void LoadOsmId(List<Region> regions){ }
    public void LoadOsmBorders(List<Region> regions) { }
}
