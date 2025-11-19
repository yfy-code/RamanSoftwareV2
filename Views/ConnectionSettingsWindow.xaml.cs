using RamanSoftwareV2.ViewModels;
using System.Windows;

namespace RamanSoftwareV2.Views
{
    public partial class ConnectionSettingsWindow : Window
    {
        public ConnectionSettingsWindow()
        {
            InitializeComponent();
            DataContext = new ConnectionSettingsViewModel();
        }
    }
}
