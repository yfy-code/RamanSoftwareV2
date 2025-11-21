using RamanSoftwareV2.Models;
using RamanSoftwareV2.Services;

namespace RamanSoftwareV2.State
{
    public static class AppState
    {
        // 全局设备设置和状态，将model中的普通类实例化为静态属性，使其在应用程序中全局可访问
        public static DeviceSettings Settings { get; set; } = new DeviceSettings();
        public static DeviceStatus Status { get; set; } = new DeviceStatus();

        // 未来：添加采集状态、模式、缓存等

        //全局设备管理器（负责连接、断开、后面也会负责采集）
        public static DeviceManager DeviceManager { get; } = new DeviceManager();
    }
}
