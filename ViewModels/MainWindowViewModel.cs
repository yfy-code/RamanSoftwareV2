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
            // 初始化命令
            ToggleLeftPanelCommand = new RelayCommand(_ => ToggleLeftPanel());

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

            EquipmentCalibrationCommand = new RelayCommand(_ =>
            {
                MessageBox.Show("Equipment Calibration 功能未实现");
            });

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
