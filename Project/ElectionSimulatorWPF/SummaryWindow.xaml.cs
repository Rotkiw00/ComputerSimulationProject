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
    /// Interaction logic for SummaryWindow.xaml
    /// </summary>
    public partial class SummaryWindow : Window
    {
        public SummaryWindow()
        {
            InitializeComponent();
        }

        private void btnBackToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var objectMainWindow = new MainWindow();
            this.Visibility = Visibility.Collapsed;
            objectMainWindow.Show();
        }
    }
}
