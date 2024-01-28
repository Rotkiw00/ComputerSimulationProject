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

			try
			{
				bool isRegionDataLoaded = BaseValues.LoadDataFromFile("RegionsData.map");
				if (isRegionDataLoaded is false) { Application.Current.Shutdown(); }
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void HandleResultMaps(ElectionType electionType, int id = 0)
		{
			SimMap.RegionClicked onClick = delegate (int id)
			{
				HandleResultMaps(electionType, id);
			};

			ShowMap(id, onClick);
		}

		private void ShowMap(int id, SimMap.RegionClicked onClick = null)
		{
			mapVisualisationGrid.Children.Clear(); // zmodyfikować canvas w XAMLu,
												   // żeby czyścić tylko buttons i mapę, a nie wszystko

			SimMap.Creator simMap = new()
			{
				Size = 900,
				Color = System.Drawing.Color.Red,
				OnClick = onClick,
				RegionId = id
			};

			var map = simMap.Create();
			var mapButtons = map.GetMap().Item2; // obsłuzyć wyjątek kiedy skończą się okręgi
			Grid.SetColumn(map, 1);
			Grid.SetRowSpan(map, 2);
			mapVisualisationGrid.Children.Add(map);

			foreach (var button in mapButtons)
			{
				Grid.SetColumn(button, 0);
				stackPanelMapButtons.Children.Add(button);
			}
		}

		private void btnBackToMainWindow_Click(object sender, RoutedEventArgs e)
		{
			var mainWndw = new MainWindow();
			this.Visibility = Visibility.Collapsed;
			mainWndw.Show();
		}

		private void btnEnterSummaryWindow_Click(object sender, RoutedEventArgs e)
		{
			var summaryWndw = new SummaryWindow();
			this.Visibility = Visibility.Collapsed;
			summaryWndw.Show();
		}

		private void btnBeginSimulation_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Rozpoczynam symulacje");
			// Wyniki dopiero po załadowaniu wynikowej mapy
			/*
			 * TUTAJ PROCES SYMULACJI
			 */
			// po załadowaniu wynikowej mapy odblokować
			// podsumowanie oraz mapy sejmu i senatu (?)
			btnEnterSummaryWindow.IsEnabled = true;
			btnSejmResultsMapLoad.Visibility = Visibility.Visible;
			btnSenatResultsMapLoad.Visibility = Visibility.Visible;
		}

		private void btnSejmResultsMapLoad_Click(object sender, RoutedEventArgs e)
		{
			HandleResultMaps(ElectionType.Sejm);
		}

		private void btnSenatResultsMapLoad_Click(object sender, RoutedEventArgs e)
		{
			HandleResultMaps(ElectionType.Senat);
		}
	}
}
