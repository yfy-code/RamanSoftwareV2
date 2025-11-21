using RamanSoftwareV2.Models;
using RamanSoftwareV2.State;
using System;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace RamanSoftwareV2.Services
{
    public class DeviceManager
    {
        private TcpClient _tcpClient;
        private SerialPort _serialPort;

        public bool IsConnected => AppState.Status.IsConnected;

        /// <summary>
        /// 连接状态变化事件：true = 已连接，false = 已断开
        /// </summary>
        public event EventHandler<bool> ConnectionStatusChanged;

        /// <summary>
        /// 根据当前设置（网络 / 串口）发起连接
        /// </summary>
        public async Task<bool> ConnectAsync(DeviceSettings settings)
        {
            // 保险起见，先清理旧连接
            DisconnectInternal();

            bool isSuccess;


            if (settings.UseNetwork)
            {
                isSuccess = await ConnectNetworkAsync(settings.IpAddress, settings.Port);
            }
            else
            {
                isSuccess = ConnectSerial(settings.ComPort, settings.BaudRate);
            }

            AppState.Status.IsConnected = isSuccess;
            OnConnectionStatusChanged(isSuccess);

            return isSuccess;
        }

        //网口连接逻辑
        private async Task<bool> ConnectNetworkAsync(string ip, int port)
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(ip, port);

                // TODO：这里可以拿 NetworkStream，启动接收任务
                // var stream = _tcpClient.GetStream();

                return true;
            }
            catch (Exception)
            {
                // TODO：后面可以改成用 SimpleLogger 记录日志
                return false;
            }
        }

        //串口连接逻辑
        private bool ConnectSerial(string portName, int baudRate)
        {
            try
            {
                _serialPort = new SerialPort(portName, baudRate);

                // TODO：这些参数可以参考你旧工程的串口设置再调整
                _serialPort.Parity = Parity.None;
                _serialPort.StopBits = StopBits.One;
                _serialPort.DataBits = 8;

                _serialPort.Open();

                // TODO：后面可以订阅 DataReceived，接收 CCD 数据
                // _serialPort.DataReceived += SerialPort_DataReceived;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //断开连接
        public void Disconnect()
        {
            DisconnectInternal();
            AppState.Status.IsConnected = false;
            OnConnectionStatusChanged(false);
        }

        private void DisconnectInternal()
        {
            try
            {
                if (_tcpClient != null)
                {
                    if (_tcpClient.Connected)
                        _tcpClient.Close();
                    _tcpClient = null;
                }
            }
            catch { }

            try
            {
                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen)
                        _serialPort.Close();
                    _serialPort.Dispose();
                    _serialPort = null;
                }
            }
            catch { }
        }


        private void OnConnectionStatusChanged(bool isConnected)
        {
            ConnectionStatusChanged?.Invoke(this, isConnected);
        }
    }
}
