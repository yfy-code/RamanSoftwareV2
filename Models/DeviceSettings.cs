namespace RamanSoftwareV2.Models
{
    public class DeviceSettings // 设备连接设置
    {
        public string IpAddress { get; set; } = "192.168.7.11";
        public int Port { get; set; } = 8000;

        public string ComPort { get; set; } = "COM4";
        public int BaudRate { get; set; } = 115200;

        public bool UseNetwork { get; set; } = true;
    }
}
