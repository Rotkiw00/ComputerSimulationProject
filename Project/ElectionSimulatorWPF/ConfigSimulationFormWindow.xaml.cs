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
			if ((String.IsNullOrEmpty(partyNameTxt1.Text) && partyColorPick1.SelectedColor == null && eurosceptismEuroentusiasmSpin1.Value == null && conservatismProgressivismSpin1.Value == null && illOrLiberalDemocracySpin1.Value == null && socialistCapitalismSpin1.Value == null) || 
				(String.IsNullOrEmpty(partyNameTxt2.Text) && partyColorPick2.SelectedColor == null && eurosceptismEuroentusiasmSpin2.Value == null && conservatismProgressivismSpin1.Value == null && illOrLiberalDemocracySpin2.Value == null && socialistCapitalismSpin2.Value == null) || 
				(String.IsNullOrEmpty(partyNameTxt3.Text) && partyColorPick3.SelectedColor == null && eurosceptismEuroentusiasmSpin3.Value == null && conservatismProgressivismSpin1.Value == null && illOrLiberalDemocracySpin3.Value == null && socialistCapitalismSpin3.Value == null) ||
				(String.IsNullOrEmpty(partyNameTxt4.Text) && partyColorPick4.SelectedColor == null && eurosceptismEuroentusiasmSpin4.Value == null && conservatismProgressivismSpin1.Value == null && illOrLiberalDemocracySpin3.Value == null && socialistCapitalismSpin4.Value == null) ||
				(String.IsNullOrEmpty(partyNameTxt5.Text) && partyColorPick5.SelectedColor == null && eurosceptismEuroentusiasmSpin5.Value == null && conservatismProgressivismSpin1.Value == null && illOrLiberalDemocracySpin5.Value == null && socialistCapitalismSpin5.Value == null) ||
				(String.IsNullOrEmpty(partyNameTxt6.Text) && partyColorPick6.SelectedColor == null && eurosceptismEuroentusiasmSpin6.Value == null && conservatismProgressivismSpin1.Value == null && illOrLiberalDemocracySpin6.Value == null && socialistCapitalismSpin6.Value == null) ||
				(String.IsNullOrEmpty(partyNameTxt7.Text) && partyColorPick7.SelectedColor == null && eurosceptismEuroentusiasmSpin7.Value == null && conservatismProgressivismSpin1.Value == null && illOrLiberalDemocracySpin7.Value == null && socialistCapitalismSpin7.Value == null) ||
				(String.IsNullOrEmpty(partyNameTxt8.Text) && partyColorPick8.SelectedColor == null && eurosceptismEuroentusiasmSpin8.Value == null && conservatismProgressivismSpin1.Value == null && illOrLiberalDemocracySpin8.Value == null && socialistCapitalismSpin8.Value == null) ||
				(String.IsNullOrEmpty(partyNameTxt9.Text) && partyColorPick9.SelectedColor == null && eurosceptismEuroentusiasmSpin9.Value == null && conservatismProgressivismSpin1.Value == null && illOrLiberalDemocracySpin9.Value == null && socialistCapitalismSpin9.Value == null) ||
				(String.IsNullOrEmpty(partyNameTxt10.Text) && partyColorPick10.SelectedColor == null && eurosceptismEuroentusiasmSpin10.Value == null && conservatismProgressivismSpin1.Value == null && illOrLiberalDemocracySpin10.Value == null && socialistCapitalismSpin10.Value == null))
			{
                System.Windows.MessageBox.Show("Aby przejść dalej trzeba wypełnić przynajmniej jedną partię.");
			}
            else
            {
				var mapSimulationWindow = new MapSimulationWindow();
				mapSimulationWindow.Show();
				this.Close();
			}		

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
