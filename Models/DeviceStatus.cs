namespace RamanSoftwareV2.Models
{
    public class DeviceStatus // 设备状态
    {
        public bool IsConnected { get; set; }
        public bool LaserOn { get; set; }
        public string CcdStatus { get; set; } = "Idle";
    }
}
