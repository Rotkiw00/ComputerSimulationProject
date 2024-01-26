using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ElectionSimulatorLibrary;
using ElectionSimulatorLibrary.WPF;

using Region = ElectionSimulatorLibrary.Region;
using Color = System.Drawing.Color;

namespace MapVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool dataExists = false;

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
                DefaultExt = ".map",
                Filter = "Simulation map files (.map)|*.map"
            };

            bool? result = dialog.ShowDialog();

            if (result != true) { return; }

            string fileName = dialog.FileName;

            bool LoadData = BaseValues.LoadDataFromFile(fileName);

            if (LoadData)
            {
                SejmButton.IsEnabled = true;
                SejmButton.Visibility = Visibility.Visible;

                SenatButton.IsEnabled = true;
                SenatButton.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Unable to load file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void SejmButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMap(ElectionType.Sejm, 0);
        }

        private void SenatButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMap(ElectionType.Senat, 0);
        }

        private void ShowMap(ElectionType electionType, int regionId)
        {
            double size = 800;
            Window mapWindow = new Window();
            mapWindow.Width = size;
            mapWindow.Height = size;
            mapWindow.Title = $"{ElectionType.Sejm.ToString("G")} - id: {regionId}";

            Window windowNavigation = new Window();
            windowNavigation.Width = 200;
            windowNavigation.Title = $"Navigation - id: {regionId}";
            var scrollViewer = new ScrollViewer();
            var stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            SimMap.RegionClicked onClick = delegate (int id)
            {
                ShowMap(electionType, id);
            };

            SimMap.Creator creator = new()
            {
                Size = size,
                ElectionType = electionType,
                MapMode = MapMode.Normal,
                RegionId = regionId,
                Color = Color.Red,
                OnClick = onClick,
                StrokeThickness = 1
            };

            // result mode test
            if (regionId == 0)
            {
                creator.MapMode = MapMode.Result;
            }

            SimMap map = creator.Create();
            if (map != null)
            {
                var mapData = map.GetMap();
                mapWindow.Content = mapData.Item1;
                if (mapData.Item2 != null)
                {
                    foreach (var button in mapData.Item2)
                    {
                        stackPanel.Children.Add(button);
                    }
                }
            }
            else
            {
                mapWindow.Close();
                windowNavigation.Close();
                return;
            }

            // result mode test
            var results = GetRandomResult(electionType);
            map.SetResult(results);

            mapWindow.SizeToContent = SizeToContent.WidthAndHeight;
            mapWindow.Show();

            scrollViewer.Content = stackPanel;
            windowNavigation.Content = scrollViewer;
            windowNavigation.Show();
        }

        private static List<Result> GetRandomResult(ElectionType type)
        {
            List<Result> result = new List<Result>();
            Random random = new Random();

            PoliticalParty p1 = new PoliticalParty();
            p1.Name = "P1";
            p1.Color = Color.Red;
            
            PoliticalParty p2 = new PoliticalParty();
            p2.Name = "P2";
            p2.Color = Color.Green;
            
            PoliticalParty p3 = new PoliticalParty();
            p3.Name = "P3";
            p3.Color = Color.Blue;
            
            if (type == ElectionType.Sejm)
                for (int i = 1; i <= 41; i++)
                {
                    double score1 = random.NextDouble();
                    double score2 = random.NextDouble();
                    double score3 = random.NextDouble();

                    Result singleResult = new Result();
                    singleResult.RegionId = i;

                    singleResult.Popularity.Add((p1, score1));
                    singleResult.Popularity.Add((p2, score2));
                    singleResult.Popularity.Add((p3, score3));

                    result.Add(singleResult);
                }
            else
                for (int i = 1; i <= 100; i++)
                {
                    double score1 = random.NextDouble();
                    double score2 = random.NextDouble();
                    double score3 = random.NextDouble();

                    Result singleResult = new Result();
                    singleResult.RegionId = i;

                    singleResult.Popularity.Add((p1, score1));
                    singleResult.Popularity.Add((p2, score2));
                    singleResult.Popularity.Add((p3, score3));

                    result.Add(singleResult);
                }

            return result;
        }
    }
}