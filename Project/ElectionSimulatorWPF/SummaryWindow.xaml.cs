using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace ElectionSimulatorWPF
{
	/// <summary>
	/// Interaction logic for SummaryWindow.xaml
	/// </summary>
	public partial class SummaryWindow : Window
    {
        public SummaryWindow()
        {
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddWpfBlazorWebView();
			Resources.Add("services", serviceCollection.BuildServiceProvider());

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
