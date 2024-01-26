using PSC.Blazor.Components.Chartjs.Models.Common;
using PSC.Blazor.Components.Chartjs.Models.Doughnut;
using PSC.Blazor.Components.Chartjs;
using PSC.Blazor.Components.Chartjs.Models.Bar;

namespace ElectionSimulatorWPF;

public partial class SimulationStats
{
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

	// Zaciągane z WPF dane
	// trzeba je obrobić i przypisać do congifa danego wykresu
	private Results _simulationResults;
	public Results SimulationResults
	{
		get => _simulationResults; 
		set
		{
			_simulationResults = value;
			StateHasChanged();
		}
	}

	private bool _isDataLoaded;
	public bool IsDataLoaded
	{
		get => _isDataLoaded; 
		set
		{
			_isDataLoaded = value;
			StateHasChanged();
		}
	}		

	protected IEnumerable<decimal?> GetElectionResults()
	{
		foreach (var dataItem in SimulationResults.ResultsSejm.WynikiProcentowe)
		{
			yield return (decimal)dataItem;
		}
	}
	// napisać metodę, która od razu przy deserializacji konwertuje double token na decimal
	protected IEnumerable<string> GetElectionPartiesNames()
	{
		foreach (var partyName in SimulationResults.ResultsSejm.EtykietyPartii)
		{
			yield return partyName;
		}
	}

	protected override Task OnInitializedAsync()
	{
		WndwState.SimulationStatsComponent = this;
		return Task.CompletedTask;
	}
}
