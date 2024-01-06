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
        List<Simulation> _simulations;

        public MainWindow()
        {
            InitializeComponent();

            ///TODO: Do wywalenia. Kiedy będziemy mieć już symulacje - dostosować pod nie
            //Jedynie istotne z punktu widzenia testowania jak wyglądają poszczególne rekordy
            _simulations = new List<Simulation>()
            {
                new()
                {
                    Name = "Symulacja Pierwsza",
                    ModificationDateTime = DateTime.Now,
                },
                new()
                {
                    Name = "Symulacja Druga",
                    ModificationDateTime = DateTime.Now,
                },
            };
            this.lstBoxOfLastSimulations.ItemsSource = _simulations;
        }

        private void btnHowToStartSim_Click(object sender, RoutedEventArgs e)
        {
            ///TODO: Do zmiany
            MessageBox.Show("Myślałem, żeby zamiast tworzenia osobnego widoku do instrukcji (bo to faktycznie trzeba będzie zrobić), to żeby przycisk routował na stronę folderu w dysku, w którym będą:\n1. Filmik z YT, który nagramy,\n2. Instrukcja w PDFie", "Info");
        }

        private void btnLoadSimulation_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "JSON files (.json)|*.json"
            };

            bool? result = dialog.ShowDialog();

            if (result != true) { return; }

            string fileName = dialog.FileName;
            ///TODO: Po wybraniu odpowiedniego pliku rozpocząć ładowanie symulacji
            ///

            MessageBox.Show($"Wybrany plik: {fileName}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnStartSimulation_Click(object sender, RoutedEventArgs e)
        {
            ///TODO: Uruchomienie widoku z mapą
            ///
        }
    }

    /*W celach testowych tworzę klasę symulującą symulacje*/
    public class Simulation
    {
        public string? Name { get; set; }
        public DateTime ModificationDateTime { get; set; }
    }
}