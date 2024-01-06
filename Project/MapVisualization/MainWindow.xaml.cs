using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WebScraper;

namespace MapVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Dictionary<string, List<Region>>? Data;

        public MainWindow()
        {
            InitializeComponent();

            SejmButton.IsEnabled = false;
            SenatButton.IsEnabled = false;
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "JSON files (.json)|*.json"
            };

            bool? result = dialog.ShowDialog();

            if (result != true) { return; }

            string fileName = dialog.FileName;

            try
            {
                var json = File.ReadAllText(fileName);
                Data = JsonSerializer.Deserialize<Dictionary<string, List<Region>>>(json);
            }
            catch
            {
                MessageBox.Show("Unable to load file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (Data == null || Data.Count != 2)
            {
                return;
            }

            if (Data.ContainsKey("sejm"))
            {
                SejmButton.IsEnabled = true;
                SejmButton.Visibility = Visibility.Visible;
            }

            if (Data.ContainsKey("senat"))
            {
                SenatButton.IsEnabled = true;
                SenatButton.Visibility = Visibility.Visible;
            }
        }

        private void SejmButton_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null) ShowCountryMap(Data["sejm"], "sejm");
        }

        private void SenatButton_Click(object sender, RoutedEventArgs e)
        {
            if (Data != null) ShowCountryMap(Data["senat"], "senat");
        }

        private void ShowCountryMap(List<Region> districts, string title)
        {
            Window countryMap = new Window();
            countryMap.Width = 1000;
            countryMap.Height = 1000;
            countryMap.Title = $"Country map - {title}";
            var canvas = new Canvas();
            canvas.Width = 1000;
            canvas.Height = 1000;

            Window countryNavigation = new Window();
            countryNavigation.Width = 200;
            countryNavigation.Title = $"Navigation - {title}";
            var scrollViewer = new ScrollViewer();
            var stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            var districtList = districts.Select((district, index) => (district.Inner, index)).ToList();

            double min_x = 9999;
            double min_y = 9999;
            double max_x = 0;
            double max_y = 0;

            foreach (var region in districtList)
            {
                foreach (var regionElement in region.Inner!)
                {
                    foreach (var coord in regionElement.Borders!)
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

            foreach (var region in districtList)
            {
                List<Polygon> localPolygonList = new();
                foreach (var regionElement in region.Inner!)
                {
                    Polygon polygon = new Polygon();
                    PointCollection points = new();
                    foreach (var coord in regionElement.Borders!)
                    {

                        points.Add(new(
                            ((coord[0] - min_x) / range_x) * 1000,
                            1000 - ((coord[1] - min_y) / range_y) * 1000));
                    }
                    polygon.Points = points;

                    polygon.Stroke = Brushes.Black;
                    polygon.StrokeThickness = 4;
                    polygon.Fill = Brushes.LightGray;

                    localPolygonList.Add(polygon);
                    canvas.Children.Add(polygon);
                }

                var onClick = () => { ShowElementMap(districts[region.index]); };

                RegionButton navButton = new(localPolygonList, districts[region.index], onClick);
                stackPanel.Children.Add(navButton);

                foreach (var polygon in localPolygonList)
                {
                    polygon.IsMouseDirectlyOverChanged += (object sender, DependencyPropertyChangedEventArgs e) =>
                    {
                        foreach (var p in localPolygonList)
                        {
                            if ((bool)e.NewValue == true)
                            {
                                p.Fill = Brushes.Red;
                            }
                            else
                            {
                                p.Fill = Brushes.LightGray;
                            }
                        }
                    };
                    polygon.MouseUp += (object sender, MouseButtonEventArgs e) =>
                    {
                        onClick?.Invoke();
                    };
                }
            }

            countryMap.Content = canvas;
            countryMap.Show();

            scrollViewer.Content = stackPanel;
            countryNavigation.Content = scrollViewer;
            countryNavigation.Show();
        }

        private void ShowElementMap(Region region)
        {
            var regionList = region.Inner;
            if (regionList == null)
                return;

            Window elementMap = new Window();
            elementMap.Width = 1000;
            elementMap.Height = 1000;
            elementMap.Title = $"Map - {region.Name}";
            var canvas = new Canvas();
            canvas.Width = 1000;
            canvas.Height = 1000;

            Window mapNavigation = new Window();
            mapNavigation.Width = 200;
            mapNavigation.Title = $"Navigation -  {region.Name}";
            var scrollViewer = new ScrollViewer();
            var stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            double min_x = 9999;
            double min_y = 9999;
            double max_x = 0;
            double max_y = 0;

            foreach (var innerRegion in regionList)
            {
                foreach (var coord in innerRegion.Borders!)
                {
                    if (coord[0] < min_x) min_x = coord[0];
                    if (coord[1] < min_y) min_y = coord[1];
                    if (coord[0] > max_x) max_x = coord[0];
                    if (coord[1] > max_y) max_y = coord[1];
                }
            }

            double range_x = max_x - min_x;
            double range_y = max_y - min_y;

            foreach (var innerRegion in regionList)
            {
                Polygon polygon = new Polygon();
                PointCollection points = new();
                foreach (var coord in innerRegion.Borders!)
                {
                    points.Add(new(
                        ((coord[0] - min_x) / range_x) * 1000,
                        1000 - ((coord[1] - min_y) / range_y) * 1000));
                }
                polygon.Points = points;

                polygon.Stroke = Brushes.Black;
                polygon.StrokeThickness = 4;
                polygon.Fill = Brushes.LightGray;

                canvas.Children.Add(polygon);

                var onClick = () => { ShowElementMap(innerRegion); };

                RegionButton navButton = new([polygon], innerRegion, onClick);
                stackPanel.Children.Add(navButton);


                polygon.IsMouseDirectlyOverChanged += (object sender, DependencyPropertyChangedEventArgs e) =>
                {

                    if ((bool)e.NewValue == true)
                    {
                        ((Polygon)sender).Fill = Brushes.Red;
                    }
                    else
                    {
                        ((Polygon)sender).Fill = Brushes.LightGray;
                    }

                };
                polygon.MouseUp += (object sender, MouseButtonEventArgs e) =>
                {
                    onClick?.Invoke();
                };

            }

            elementMap.Content = canvas;
            elementMap.Show();

            scrollViewer.Content = stackPanel;
            mapNavigation.Content = scrollViewer;
            mapNavigation.Show();
        }
    }
}