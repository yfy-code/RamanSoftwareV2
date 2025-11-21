using OxyPlot;
using OxyPlot.Series;
using RamanSoftwareV2.Commands;
using RamanSoftwareV2.Core;
using RamanSoftwareV2.State;
using RamanSoftwareV2.Views;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace RamanSoftwareV2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand EquipmentCalibrationCommand { get; }
        public ICommand OpenConnectionSettingsCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        public string ConnectButtonText
        {
            get=> AppState.Status.IsConnected ? "Disconnect" : "Connect";
        }

        public ICommand ConnectButtonCommand
        {
            get => AppState.Status.IsConnected ? DisconnectCommand : ConnectCommand;
        }


        /*-----------------------------------------
         * 左侧控制面板宽度
         -----------------------------------------*/
        private double _leftPanelWidth = 0;
        public double LeftPanelWidth
        {
            get => _leftPanelWidth;
            set { _leftPanelWidth = value; OnPropertyChanged(); }
        }

        /*-----------------------------------------
         * 状态栏绑定字段
         -----------------------------------------*/
        public string ConnectionStatus
        {
            get => AppState.Status.IsConnected ? "Connected" : "Not Connected";
        }

        public string LaserStatus
        {
            get => AppState.Status.LaserOn ? "Laser On" : "Laser Off";
        }

        private string _systemTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string SystemTime
        {
            get => _systemTime;
            set { _systemTime = value; OnPropertyChanged(); }
        }

        /*-----------------------------------------
         * 图表模型
         -----------------------------------------*/
        public PlotModel RawPlotModel { get; }
        public PlotModel PreprocessedPlotModel { get; }
        public PlotModel AnalyzedPlotModel { get; }

        private readonly DispatcherTimer _timeTimer;

        /*-----------------------------------------
         * 按钮命令
         -----------------------------------------*/
        public ICommand ToggleLeftPanelCommand { get; }


        /*-----------------------------------------
         * 构造函数
         -----------------------------------------*/
        public MainWindowViewModel()
        {
            // 左侧面板展开/折叠
            ToggleLeftPanelCommand = new RelayCommand(_ => ToggleLeftPanel());

            // 连接命令
            ConnectCommand= new RelayCommand(async _ =>
            {
                bool ok = await AppState.DeviceManager.ConnectAsync(AppState.Settings);

                // 通知 UI 刷新状态栏文字
                OnPropertyChanged(nameof(ConnectionStatus));
                OnPropertyChanged(nameof(ConnectButtonText));
                OnPropertyChanged(nameof(ConnectButtonCommand));

                if (!ok)
                {
                    MessageBox.Show("连接失败，请检查连接设置。",
                        "Connection",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });

            // 断开命令
            DisconnectCommand = new RelayCommand(_ =>
            {
                AppState.DeviceManager.Disconnect();
                // 通知 UI 刷新状态栏文字
                OnPropertyChanged(nameof(ConnectionStatus));
                OnPropertyChanged(nameof(ConnectButtonText));
                OnPropertyChanged(nameof(ConnectButtonCommand));
            });

            //订阅全局连接状态变化事件（比如未来自动断开 / 出错时也能刷新 UI）
            AppState.DeviceManager.ConnectionStatusChanged += (s, connected) =>
            {
                OnPropertyChanged(nameof(ConnectionStatus));
                OnPropertyChanged(nameof(ConnectButtonText));
                OnPropertyChanged(nameof(ConnectButtonCommand));
            };

            // 构造图表
            RawPlotModel = CreateDemoPlot("Raw Spectrum", OxyColors.SteelBlue);
            PreprocessedPlotModel = CreateDemoPlot("Preprocessed Spectrum", OxyColors.SeaGreen);
            AnalyzedPlotModel = CreateDemoPlot("Analyzed Spectrum", OxyColors.IndianRed);

            // 系统时间定时器
            _timeTimer = new DispatcherTimer();
            _timeTimer.Interval = TimeSpan.FromSeconds(1);
            _timeTimer.Tick += (s, e) =>
            {
                SystemTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            };
            _timeTimer.Start();

            //设备校准命令（未实现）
            EquipmentCalibrationCommand = new RelayCommand(_ =>
            {
                MessageBox.Show("Equipment Calibration 功能未实现");
            });

            //打开连接设置窗口命令
            OpenConnectionSettingsCommand = new RelayCommand(_ =>
            {
                var win = new ConnectionSettingsWindow();
                win.Owner = Application.Current.MainWindow;
                win.ShowDialog();
            });

        }

        /*-----------------------------------------
         * 示例图表
         -----------------------------------------*/
        private PlotModel CreateDemoPlot(string title, OxyColor color)
        {
            var model = new PlotModel { Title = title };
            var line = new LineSeries { Color = color, StrokeThickness = 1.3 };

            for (int i = 0; i < 100; i++)
                line.Points.Add(new DataPoint(i, Math.Sin(i * 0.1) + 1));

            model.Series.Add(line);
            return model;
        }

        /*-----------------------------------------
         * 控制左侧面板展开/折叠
         -----------------------------------------*/
        private void ToggleLeftPanel()
        {
            LeftPanelWidth = (LeftPanelWidth > 0) ? 0 : 260;
        }
    }
}
