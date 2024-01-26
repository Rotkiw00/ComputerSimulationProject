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
		private int gridsPartyCreator;
		private List<TextBox> dynamicTextBoxes = [];

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

			var mapSimulationWindow = new MapSimulationWindow();
			this.Visibility = Visibility.Collapsed;
			mapSimulationWindow.Show();

			// Zapisywanie kofiguracji ??
		}
		
		private void btnCreatePartiesControl_Click(object sender, RoutedEventArgs e)
		{
			btnCreatePartiesControl.IsEnabled = false;

			if (!String.IsNullOrEmpty(txtParties.Text))
			{
				if (int.TryParse(txtParties.Text, out gridsPartyCreator))
				{
					for (int i = 0; i < gridsPartyCreator; i++)
					{
						CreatePartyControlForms();
					}
				}
			}
		}

		private void CreatePartyControlForms()
		{
			Grid grid = new()
			{
				Height = 50
			};

			ColumnDefinition column1 = new()
			{
				Width = new GridLength(1, GridUnitType.Star)
			};
			ColumnDefinition column2 = new()
			{
				Width = new GridLength(1, GridUnitType.Star)
			};

			grid.ColumnDefinitions.Add(column1);
			grid.ColumnDefinitions.Add(column2);

			Label label = new()
			{
				Content = "Ile partii politycznych do symulacji ?",
				VerticalAlignment = VerticalAlignment.Center,
				FontSize = 25,
				Margin = new Thickness(10, 0, 0, 0),
				Height = 44
			};
			Grid.SetColumn(label, 0);
			grid.Children.Add(label);

			TextBox textBox = new()
			{
				Name = "txtParty",
				VerticalAlignment = VerticalAlignment.Center,
				FontSize = 23,
				Margin = new Thickness(522, 0, 392, 0)
			};
			Grid.SetColumnSpan(textBox, 2);
			grid.Children.Add(textBox);
			dynamicTextBoxes.Add(textBox);

			Button button = new()
			{
				Name = "btnCreatePartiesControl",
				Content = "Zatwierdź",
				Margin = new Thickness(216, 9, 81, 10),
				FontSize = 20
			};
			button.Click += btnCreatePartiesControl_Click;
			Grid.SetColumn(button, 1);
			grid.Children.Add(button);

			stackPanelView.Children.Add(grid);
		}
	}
}
