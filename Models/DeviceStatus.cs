namespace RamanSoftwareV2.Models
{
    public class DeviceStatus
    {
        public bool IsConnected { get; set; }
        public bool LaserOn { get; set; }
        public string CcdStatus { get; set; } = "Idle";
    }
}
