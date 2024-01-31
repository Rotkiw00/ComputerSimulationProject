﻿using ElectionSimulatorLibrary;
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
			SimMap.Creator simMapCreator = new()
			{
				Size = 900,
				Color = System.Drawing.Color.Red,
				OnClick = onClick,
				RegionId = id
			};

			var simMap = simMapCreator.Create();
			if (simMap is not null)
			{
				mapLayoutGrid.Children.Clear();

				Grid.SetColumn(simMap, 1);
				Grid.SetRowSpan(simMap, 2);
				mapLayoutGrid.Children.Add(simMap);

				var simMapButtons = simMap.GetMap().Item2;
				if (simMapButtons is not null)
				{
					stackPanelMapButtons.Children.Clear();

					foreach (var mapButton in simMapButtons)
					{
						Grid.SetColumn(mapButton, 0);
						stackPanelMapButtons.Children.Add(mapButton);
					}
				}
			}
		}

		private void btnBackToMainWindow_Click(object sender, RoutedEventArgs e)
		{
			var mainWndw = new MainWindow();
			mainWndw.Show();
			this.Close();
		}

		private void btnEnterSummaryWindow_Click(object sender, RoutedEventArgs e)
		{
			var summaryWndw = new SummaryWindow();
			summaryWndw.Show();
			// nie zamykam okna z mapą 
			//this.Close(); 
		}

		private void btnBeginSimulation_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Rozpoczynam symulacje");
			// Wyniki dopiero po załadowaniu wynikowej mapy
			/*
			 * TUTAJ PROCES SYMULACJI (?)
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
		// nie wiem czemu nie działa odpowiednie rzutowanie na daną mapę
		private void btnSenatResultsMapLoad_Click(object sender, RoutedEventArgs e)
		{
			HandleResultMaps(ElectionType.Senat);
		}
	}
}
