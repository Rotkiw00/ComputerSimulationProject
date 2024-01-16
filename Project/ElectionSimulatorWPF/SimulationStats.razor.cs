using PSC.Blazor.Components.Chartjs.Models.Common;
using PSC.Blazor.Components.Chartjs.Models.Doughnut;
using PSC.Blazor.Components.Chartjs;
using PSC.Blazor.Components.Chartjs.Models.Bar;

namespace ElectionSimulatorWPF
{
	public partial class SimulationStats
	{
		private BarChartConfig _barChartConfig1;
		private Chart _barChart1;

		private DoughnutChartConfig _doughnutChartConfig2;
		private Chart _doughnutChart2;

		private DoughnutChartConfig _doughnutChartConfig3;
		private Chart _doughnutChart3;

		protected override async Task OnInitializedAsync()
		{
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

			_barChartConfig1.Data.Labels = ["January", "February", "March"];
			List<decimal?> data1 = [100, 200, 400];

			_barChartConfig1.Data.Datasets.Add(new BarDataset()
			{
				Data = data1,
				BackgroundColor = ["yellow", "red", "blue"],
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
		}		
	}
}
