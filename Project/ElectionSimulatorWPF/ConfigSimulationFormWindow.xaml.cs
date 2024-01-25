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
	/// Interaction logic for ConfigSimulationFormWindow.xaml
	/// </summary>
	public partial class ConfigSimulationFormWindow : Window
	{
		public ConfigSimulationFormWindow()
		{
			InitializeComponent();
		}

		private void btnBackToMainWindow_Click(object sender, RoutedEventArgs e)
		{
			var objectMainWindow = new MainWindow();
			this.Visibility = Visibility.Collapsed;
			objectMainWindow.Show();
		}

		private void btnSaveConfigForm_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Zapisywanie konfiguracji do symulacji", "Info");
		}
	}
}
