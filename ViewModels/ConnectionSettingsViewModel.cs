using RamanSoftwareV2.Commands;
using RamanSoftwareV2.Core;
using RamanSoftwareV2.State;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows;
using System.Windows.Input;

namespace RamanSoftwareV2.ViewModels
{
    public class ConnectionSettingsViewModel : ViewModelBase
    {
        /*---------------------------------------------------------
         * 绑定属性
         ---------------------------------------------------------*/

        // 0 = Network Port, 1 = Serial Port
        private int _modeIndex = AppState.Settings.UseNetwork ? 0 : 1;
        public int ModeIndex
        {
            get => _modeIndex;
            set
            {
                if (_modeIndex != value)
                {
                    _modeIndex = value;
                    OnPropertyChanged();

                    // 触发 UI 更新 Visibility
                    OnPropertyChanged(nameof(IsNetworkMode));
                    OnPropertyChanged(nameof(IsSerialMode));
                }
            }
        }

        public bool IsNetworkMode => ModeIndex == 0;
        public bool IsSerialMode => ModeIndex == 1;


        // ----- Network -----

        private string _ipAddress = AppState.Settings.IpAddress;
        public string IpAddress
        {
            get => _ipAddress;
            set { _ipAddress = value; OnPropertyChanged(); }
        }

        private int _port = AppState.Settings.Port;
        public int Port
        {
            get => _port;
            set { _port = value; OnPropertyChanged(); }
        }


        // ----- Serial -----

        public ObservableCollection<string> ComPorts { get; } =
            new ObservableCollection<string>(SerialPort.GetPortNames());

        private string _selectedCom = AppState.Settings.ComPort;
        public string SelectedCom
        {
            get => _selectedCom;
            set { _selectedCom = value; OnPropertyChanged(); }
        }

        private string _selectedBaud = "115200";
        public string SelectedBaud
        {
            get => _selectedBaud;
            set { _selectedBaud = value; OnPropertyChanged(); }
        }


        /*---------------------------------------------------------
         * 命令
         ---------------------------------------------------------*/
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }


        /*---------------------------------------------------------
         * 构造函数
         ---------------------------------------------------------*/
        public ConnectionSettingsViewModel()
        {
            SaveCommand = new RelayCommand(_ => SaveSettings());
            CancelCommand = new RelayCommand(win => (win as Window)?.Close());
        }


        /*---------------------------------------------------------
         * 保存设置
         ---------------------------------------------------------*/
        private void SaveSettings()
        {
            // 保存到 AppState 全局状态
            AppState.Settings.UseNetwork = IsNetworkMode;

            AppState.Settings.IpAddress = IpAddress;
            AppState.Settings.Port = Port;

            AppState.Settings.ComPort = SelectedCom;
            AppState.Settings.BaudRate = int.Parse(SelectedBaud);

            MessageBox.Show("Settings saved!",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
