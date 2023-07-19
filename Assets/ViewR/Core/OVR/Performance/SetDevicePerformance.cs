using UnityEngine;

namespace ViewR.Core.OVR.Performance
{
    /// <summary>
    /// An accessor to <see cref="DevicePerformanceManager"/>
    /// </summary>
    public class SetDevicePerformance : MonoBehaviour
    {
        public void SetCpuPerformanceDefault() => DevicePerformanceManager.RestoreDefaultCPU();
        public void SetCpuPerformanceHigh() => DevicePerformanceManager.SetPerformance(cpuLevel: (OVRPlugin.ProcessorPerformanceLevel) 2);
        public void SetGpuPerformanceDefault() => DevicePerformanceManager.RestoreDefaultGPU();
        public void SetGpuPerformanceHigh() => DevicePerformanceManager.SetPerformance(gpuLevel: (OVRPlugin.ProcessorPerformanceLevel) 4);
        public void SetFixedFoveatedRenderingLevelDefault() => DevicePerformanceManager.RestoreDefaultFfrLevel();
        public void SetFixedFoveatedRenderingLevelMedium() => DevicePerformanceManager.SetPerformance(fixedFoveatedRenderingLevel: OVRPlugin.FoveatedRenderingLevel.Medium);
        public void SetDynamicFixedFoveatedRenderingLevelDefault() => DevicePerformanceManager.RestoreDefaultUseDynmaicFfr();
        public void SetDynamicFixedFoveatedRenderingLevelOn() => DevicePerformanceManager.SetPerformance(useDynamicFixedFoveatedRendering: true);
    }
}