using System.IO;
using System.Text.Json;
using System.Windows;
using Blazored.Modal;
using Microsoft.Extensions.DependencyInjection;

namespace ElectionSimulatorWPF;

/// <summary>
/// Interaction logic for SummaryWindow.xaml
/// </summary>
public partial class SummaryWindow : Window
{
	public SummaryWindow()
	{
		var serviceCollection = new ServiceCollection();
		serviceCollection.AddWpfBlazorWebView();
		serviceCollection.AddBlazoredModal();
		Resources.Add("services", serviceCollection.BuildServiceProvider());

		InitializeComponent();
		WndwState.SummaryWindow = this;
	}

	private void btnBackToMainWindow_Click(object sender, RoutedEventArgs e)
	{
		var objectMainWindow = new MainWindow();
		this.Visibility = Visibility.Collapsed;
		objectMainWindow.Show();
	}

	private void btnLoadAndShowRezults_Click(object sender, RoutedEventArgs e)
	{
		var dialog = new Microsoft.Win32.OpenFileDialog()
		{
			DefaultExt = ".json",
			Filter = "JSON (.json)|*.json|All files|*.*"
		};

		bool? isOpen = dialog.ShowDialog();
		if (isOpen is true)
		{
			string rezultsFilePath = dialog.FileName;
			if (Path.GetExtension(rezultsFilePath) is not ".json")
			{
				MessageBox.Show("Błędny format pliku. Nie można wyświetlić wyniku", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				RetrieveDataFromJson(rezultsFilePath);
			}
		}
		else
		{
			MessageBox.Show("Nie wybrano pliku. Nie można wyświetlić wyniku", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
	}

	// metoda zczytująca jsona. Powinna zwracać dane do Blazora i tam się nimi już zajmować
	private static void RetrieveDataFromJson(string pathData)
	{
		string json = File.ReadAllText(pathData);
		var results = JsonSerializer.Deserialize<Results>(json);
		// przekazać w metodzie
		WndwState.SimulationStatsComponent.SimulationResults = results;
		WndwState.SimulationStatsComponent.IsDataLoaded = true;
	}
}

// referencja 'do' i 'z' WPF względem Blazora
public static class WndwState
{ 
	public static SummaryWindow SummaryWindow { get; set; } 
	public static SimulationStats SimulationStatsComponent { get; set; }
}

// testowo sprawdzić ładowanie JSONa. Bazowe wyniki. TODO: do zmiany
// i wywalić do jakiegoś folderu typu model albo inne podejście
public class ResultsSejm
{
	public List<string> EtykietyPartii { get; set; }
	public List<double> WynikiProcentowe { get; set; }
	public List<object> RozkladMandatow { get; set; }
}

public class ResultsSenat
{
	public List<object> RozkladMandatow { get; set; }
}

public class Results
{
	public ResultsSejm ResultsSejm { get; set; }
	public ResultsSenat ResultsSenat { get; set; }
}
