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

            bool LoadData = SimMap.LoadDataFromFile(fileName);

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

            SimMap map = creator.Create();
            if(map != null)
            {
                var mapData = map.GetMap();
                mapWindow.Content = mapData.Item1;
                if(mapData.Item2 != null)
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

           mapWindow.SizeToContent = SizeToContent.WidthAndHeight;
            mapWindow.Show();

            scrollViewer.Content = stackPanel;
            windowNavigation.Content = scrollViewer;
            windowNavigation.Show();
        }
    }
}