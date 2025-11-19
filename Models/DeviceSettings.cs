namespace RamanSoftwareV2.Models
{
    public class DeviceSettings
    {
        public string IpAddress { get; set; } = "192.168.7.11";
        public int Port { get; set; } = 1234;

        public string ComPort { get; set; } = "COM1";
        public int BaudRate { get; set; } = 115200;

        public bool UseNetwork { get; set; } = true;
    }
}
