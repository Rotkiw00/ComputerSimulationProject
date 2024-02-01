using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectionSimulatorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Application.Current.Shutdown();
		}

		private void btnHowToStartSim_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://github.com/Rotkiw00/ComputerSimulationProject/blob/main/Sprawozdanie.pdf";
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });

        }

        private void btnStartSimulation_Click(object sender, RoutedEventArgs e)
        {
            //var configSimFormWindow = new ConfigSimulationFormWindow();
            this.Visibility = Visibility.Collapsed;
            //configSimFormWindow.Show();

            var mapSimulationWindow = new MapSimulationWindow();
            mapSimulationWindow.Show();
        }
    }
}