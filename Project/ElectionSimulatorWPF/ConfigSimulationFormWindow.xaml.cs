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
		private int gridsPartyCreator;
		private List<TextBox> dynamicTextBoxes = [];
		private List<Slider> dynamicSliders = [];
		private List<ColorPicker> dynamicColorPickeres = [];

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
			var mapSimulationWindow = new MapSimulationWindow();
			this.Visibility = Visibility.Collapsed;
			mapSimulationWindow.Show();

			
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
						CreatePartyControlForms(i);
					}
				}
			}
		}

		private void CreatePartyControlForms(int id)
		{
			Grid grid = new()
			{
				Height = 100,
				Margin = new Thickness(0,10,0,0),
			};

			ColumnDefinition column1 = new()
			{
				Width = new GridLength(1, GridUnitType.Star)
			};
			ColumnDefinition column2 = new()
			{
				Width = new GridLength(1, GridUnitType.Star)
			};
			RowDefinition row1 = new()
			{
				Height = new GridLength(1, GridUnitType.Star)
			};
			RowDefinition row2 = new()
			{
				Height = new GridLength(1, GridUnitType.Star)
			};
			RowDefinition row3 = new()
			{
				Height = new GridLength(1, GridUnitType.Star)
			};

			grid.ColumnDefinitions.Add(column1);
			grid.ColumnDefinitions.Add(column2);
			grid.RowDefinitions.Add(row1);
			grid.RowDefinitions.Add(row2);
			grid.RowDefinitions.Add(row3);

			Label label = new()
			{
				Content = $"#{id + 1} Partia. Nazwa:",
				VerticalAlignment = VerticalAlignment.Center,
				FontSize = 25,
				Margin = new Thickness(10, 0, 0, 0),
				Height = 40
			};
			Grid.SetColumn(label, 0);
			grid.Children.Add(label);

			TextBox textBox = new()
			{
				Name = "txtParty",
				VerticalAlignment = VerticalAlignment.Center,
				FontSize = 23,
				Margin = new Thickness(10, 0, 10, 0)
			};
			Grid.SetColumn(textBox, 1);
			grid.Children.Add(textBox);
			dynamicTextBoxes.Add(textBox);

			Slider slider = new()
			{
				Name = "sliderParty",
				VerticalAlignment = VerticalAlignment.Center,
				Minimum = -100,
				Maximum = 100,
				TickFrequency = 100,
				IsSnapToTickEnabled = true,
				TickPlacement = System.Windows.Controls.Primitives.TickPlacement.Both
			};
			Grid.SetRow(slider, 1);
			Grid.SetColumnSpan(slider, 2);
			grid.Children.Add(slider);
			dynamicSliders.Add(slider);

			Label colorLabel = new()
			{
				Content = $"#{id + 1} Kolor partii:",
				VerticalAlignment = VerticalAlignment.Center,
				FontSize = 25,
				Margin = new Thickness(10, 0, 0, 0),
				Height = 40
			};
			Grid.SetColumn(colorLabel, 0);
			Grid.SetRow(colorLabel, 2);
			grid.Children.Add(colorLabel);

			ColorPicker colorPicker = new()
			{
				Name = "partyColorPicker",
				DisplayColorAndName = true,
				Height = 20,
				VerticalAlignment= VerticalAlignment.Top,
				Margin = new Thickness(10, 10, 10, 10)
			};
			Grid.SetRow(colorPicker, 2);
			Grid.SetColumn(colorPicker, 1);
			grid.Children.Add(colorPicker);
			dynamicColorPickeres.Add(colorPicker);

			stackPanelView.Children.Add(grid);
		}
	}
}
