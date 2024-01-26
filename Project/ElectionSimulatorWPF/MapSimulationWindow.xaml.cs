using ElectionSimulatorLibrary;
using ElectionSimulatorLibrary.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ElectionSimulatorWPF
{
	/// <summary>
	/// Interaction logic for MapSimulationWindow.xaml
	/// </summary>
	public partial class MapSimulationWindow : Window
	{
		public MapSimulationWindow()
		{
			InitializeComponent();

			bool isRegionDataLoaded = BaseValues.LoadDataFromFile("RegionsData.map");

			SimMap.Creator simMap = new()
			{
				Size = 1000,
				Color = System.Drawing.Color.Red
			};

			if (isRegionDataLoaded)
			{
				var map = simMap.Create();
				var mapButtons = map.GetMap().Item2;
				Grid.SetColumn(map, 1);

				foreach (var button in mapButtons)
				{
					Grid.SetColumn(button, 0);
					stackPanelMapButtons.Children.Add(button);
				}

				mapVisualisationGrid.Children.Add(map);
			}
			else
			{
				MessageBox.Show("Nie można załadować danych do mapy.");
			}
		}

		private void btnBackToMainWindow_Click(object sender, RoutedEventArgs e)
		{
			var mainwndw = new MainWindow();
			this.Visibility = Visibility.Collapsed;
			mainwndw.Show();
		}
	}
}
