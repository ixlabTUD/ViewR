using UnityEngine.Events;

namespace ViewR.StatusManagement
{
    /// <summary>
    /// For future devices.
    /// </summary>
    public enum ClientDevice
    {
        VR,
        AR
    }
    
    /// <summary>
    /// Allows to keep track of our device type.
    /// Other methods can subscribe to changes.
    /// </summary>
    public static class ClientDeviceType
    {
        public static ClientDevice CurrentClientDevice { get; private set; } = ClientDevice.VR;

        #region Events

        public delegate void DeviceTypeUpdatedHandler(ClientDevice clientDevice);
        public static event DeviceTypeUpdatedHandler DeviceTypeUpdated;

        #endregion

        /// <summary>
        /// Sets the current status and fires the respective event. <see cref="DeviceTypeUpdated"/>
        /// </summary>
        public static void SetStatus(ClientDevice clientDevice)
        {
            CurrentClientDevice = clientDevice;
            DeviceTypeUpdated?.Invoke(CurrentClientDevice);
        }
    }
}
