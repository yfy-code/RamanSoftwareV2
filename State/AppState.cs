using RamanSoftwareV2.Models;

namespace RamanSoftwareV2.State
{
    public static class AppState
    {
        public static DeviceSettings Settings { get; set; } = new DeviceSettings();
        public static DeviceStatus Status { get; set; } = new DeviceStatus();

        // 未来：添加采集状态、模式、缓存等
    }
}
