using PSC.Blazor.Components.Chartjs.Models.Common;
using PSC.Blazor.Components.Chartjs.Models.Doughnut;
using PSC.Blazor.Components.Chartjs;
using PSC.Blazor.Components.Chartjs.Models.Bar;

namespace ElectionSimulatorWPF
{
	public partial class SimulationStats
	{
		// Zaciągane z WPF dane
		// trzeba je obrobić i przypisać do congifa danego wykresu
		private Results _simulationResults;
		public Results SimulationResults
		{
			get => _simulationResults; set
			{
				_simulationResults = value;
				StateHasChanged();
			}
		}

		private bool _isDataLoaded;
		public bool IsDataLoaded
		{
			get => _isDataLoaded; set
			{
				_isDataLoaded = value;
				StateHasChanged();
			}
		}

		// wykresy z tego nugeta muszą być obiektami
		// TODO: trzeba ogarnąć, żeby one nie były tak luźno
		// w kodzie
		private BarChartConfig _barChartConfig1;
		private Chart _barChart1;

		private DoughnutChartConfig _doughnutChartConfig2;
		private Chart _doughnutChart2;

		private DoughnutChartConfig _doughnutChartConfig3;
		private Chart _doughnutChart3;

		// wykresy NIE mogą być wyświetlane przy inicjalizacji WebView
		// TODO: Dorzucić jakąś flagę, wskazującą na otrzymanie danych i możliwość wyświetlenia wykresów
		protected override Task OnInitializedAsync()
		{
			AppState.SimulationStatsComponent = this;

			_barChartConfig1 = new BarChartConfig()
			{
				Options = new Options()
				{
					Plugins = new Plugins()
					{
						Legend = new Legend()
						{
							Align = Align.Center,
							Display = false,
							Position = LegendPosition.Right
						}
					},
				}
			};

			_barChartConfig1.Data.Labels = ["Koalicja Obywatelska",
				"Zjednoczona Prawica",
				"Trzecia Droga",
				"Nowa Lewica",
				"Konfederacja",
				"Bezpartyjni Samorządowcy"];
			List<decimal?> data1 = [100, 200, 400];

			_barChartConfig1.Data.Datasets.Add(new BarDataset()
			{
				Data = data1,
				BorderWidth = 1
			});

			// ========================================================

			_doughnutChartConfig2 = new DoughnutChartConfig()
			{
				Options = new Options()
				{
					Responsive = true,
					MaintainAspectRatio = false,
					Plugins = new Plugins()
					{
						Legend = new Legend()
						{
							Display = true,
							Position = LegendPosition.Left
						}
					}
				}
			};

			_doughnutChartConfig2.Data.Labels = ["January", "February", "March"];
			List<decimal?> data2 = [100, 200, 400];

			_doughnutChartConfig2.Data.Datasets.Add(new DoughnutDataset()
			{
				Data = data2,
				BackgroundColor = ["yellow", "red", "blue"],
				HoverOffset = 4
			});

			// ========================================================

			_doughnutChartConfig3 = new DoughnutChartConfig()
			{
				Options = new Options()
				{
					Responsive = true,
					MaintainAspectRatio = false,
					Plugins = new Plugins()
					{
						Legend = new Legend()
						{
							Display = true,
							Position = LegendPosition.Left
						}
					}
				}
			};

			_doughnutChartConfig3.Data.Labels = ["January", "February", "March"];
			List<decimal?> data = [100, 200, 400];

			_doughnutChartConfig3.Data.Datasets.Add(new DoughnutDataset()
			{
				Data = data,
				BackgroundColor = ["yellow", "red", "blue"],
				HoverOffset = 4
			});
			return Task.CompletedTask;
		}		
	}
}
