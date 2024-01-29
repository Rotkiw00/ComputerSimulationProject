using ElectionSimulatorLibrary;
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
using Xceed.Wpf.Toolkit;

namespace ElectionSimulatorWPF
{
	/// <summary>
	/// Interaction logic for ConfigSimulationFormWindow.xaml
	/// </summary>
	public partial class ConfigSimulationFormWindow : Window
	{
		/* Zebrane dane z formularzu politycznego dodać jako obiekt PoliticalParty
		 * a następnie przekazać go do listy poniżej
		 * obsłużenie formularza, że nie wszystkie muszą być wypełnione (?)
		 * wtedy uruchamiana jest bazowa (?)
		 */
		public List<PoliticalParty> politicalPartyList; 

		public ConfigSimulationFormWindow()
		{
			InitializeComponent();
		}

		private void btnBackToMainWindow_Click(object sender, RoutedEventArgs e)
		{
			var objectMainWindow = new MainWindow();
			objectMainWindow.Show();
			this.Close();
		}

		private void btnSaveConfigForm_Click(object sender, RoutedEventArgs e)
		{
			var mapSimulationWindow = new MapSimulationWindow();
			mapSimulationWindow.Show();
			this.Close();
			/*
			 przekazanie danych z formularza do symulacji (?)
		     czy zapisanie do pliku .. (?) i później odczyt (?)
			 */
		}

		private void btnLoadBaseSimulation_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
