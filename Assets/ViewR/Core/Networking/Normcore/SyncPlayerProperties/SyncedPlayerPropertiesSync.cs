using Normal.Realtime;
using UnityEngine;
using ViewR.Core.UI.MainUI.UI.ConfigMenu;
using ViewR.HelpersLib.Extensions.General;
using ViewR.Managers;
using ViewR.StatusManagement;

namespace ViewR.Core.Networking.Normcore.SyncPlayerProperties
{
    public class SyncedPlayerPropertiesSync : RealtimeComponent<SyncedPlayerPropertiesModel>
    {
        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        #region Public getters

        public string GetCurrentUserName() => model.userName;
        public ClientPhysicalLocation GetCurrentPhysicalLocation() => model.physicalLocation;
        public ClientDevice GetCurrentClientDeviceType() => model.clientDeviceType;
        public int GetCurrentUserID() => model.userID;
        public int GetCurrentUserRole() => model.userRole;
        public bool GetCurrentMegaphonePermission() => model.megaphonePermission;

        #endregion

        private void OnEnable()
        {
            // Subscribe
            ConfigUserName.UserNameDidChange += HandleNewLocalUserName;
            ClientDeviceType.DeviceTypeUpdated += HandleNewClientDevice;
            ClientPhysicalLocationState.ClientPhysicalLocationDidChange += HandleNewPhysicalLocation;
        }
        
        private void OnDisable()
        {
            // Unsubscribe
            ConfigUserName.UserNameDidChange -= HandleNewLocalUserName;
            ClientDeviceType.DeviceTypeUpdated -= HandleNewClientDevice;
            ClientPhysicalLocationState.ClientPhysicalLocationDidChange -= HandleNewPhysicalLocation;
        }

        #region Normcore logic

        protected override void OnRealtimeModelReplaced(SyncedPlayerPropertiesModel previousModel,
            SyncedPlayerPropertiesModel currentModel)
        {
            if (previousModel != null)
            {
                // Unsubscribe
                previousModel.userNameDidChange -= OnUserNameDidChange;
                previousModel.clientDeviceTypeDidChange -= OnClientDeviceTypeDidChange;
                previousModel.physicalLocationDidChange -= OnPhysicalLocationDidChange;
                previousModel.userRoleDidChange -= OnUserRoleDidChange;
                previousModel.userIDDidChange -= OnUserIDDidChange;
                previousModel.megaphonePermissionDidChange -= OnMegaphonePermissionDidChange;
            }

            if (currentModel != null)
            {
                // Ensure fresh models have appropriate values.
                if (model.isFreshModel)
                {
                    currentModel.userName = UserConfig.UserName;
                    currentModel.clientDeviceType = ClientDeviceType.CurrentClientDevice;
                    currentModel.physicalLocation = ClientPhysicalLocationState.CurrentClientPhysicalLocation;
                }

                // Subscribe
                currentModel.userNameDidChange += OnUserNameDidChange;
                currentModel.clientDeviceTypeDidChange += OnClientDeviceTypeDidChange;
                currentModel.physicalLocationDidChange += OnPhysicalLocationDidChange;
                currentModel.userRoleDidChange += OnUserRoleDidChange;
                currentModel.userIDDidChange += OnUserIDDidChange;
                currentModel.megaphonePermissionDidChange += OnMegaphonePermissionDidChange;
            }
        }

        #endregion

        #region Callbacks based on network changes

        private void OnUserNameDidChange(SyncedPlayerPropertiesModel syncedPlayerPropertiesModel, string value)
        {
            // Pass on invocation
            InvokeUserNameDidChange(value);
        }

        private void OnPhysicalLocationDidChange(SyncedPlayerPropertiesModel syncedPlayerPropertiesModel, ClientPhysicalLocation value)
        {
            // Pass on invocation
            InvokePhysicalLocationDidChange(value);
        }

        private void OnClientDeviceTypeDidChange(SyncedPlayerPropertiesModel syncedPlayerPropertiesModel, ClientDevice value)
        {
            // Pass on invocation
            InvokeClientDeviceTypeDidChange(value);
        }

        private void OnUserIDDidChange(SyncedPlayerPropertiesModel syncedPlayerPropertiesModel, int value)
        {
            // Pass on invocation
            InvokeUserIDDidChange(value);
            Debug.LogWarning("This is a placeholder. It is only foresight, as updating Normcore classes is a pain.");
        }

        private void OnUserRoleDidChange(SyncedPlayerPropertiesModel syncedPlayerPropertiesModel, int value)
        {
            // Pass on invocation
            InvokeUserRoleDidChange(value);
            Debug.LogWarning("This is a placeholder. It is only foresight, as updating Normcore classes is a pain.");
        }

        private void OnMegaphonePermissionDidChange(SyncedPlayerPropertiesModel syncedPlayerPropertiesModel, bool value)
        {
            // Pass on invocation
            InvokeMegaphonePermissionDidChange(value);
            Debug.LogWarning("This is a placeholder. It is only foresight, as updating Normcore classes is a pain.");
        }

        #endregion

        #region Change model values

        /// <summary>
        /// Applies a new user name to the class.
        /// </summary>
        private void HandleNewLocalUserName(string newName)
        {
            // Bail if this is not ours!
            if(!model.isOwnedLocallySelf) 
                return;
            
            // Else: Apply new value.
            if (debugging)
                Debug.Log($"Setting new userName: {newName}".Green().StartWithFrom(GetType()), this);
            
            model.userName = newName;
        }

        /// <summary>
        /// Applies a new <see cref="ClientDevice"/> to the class.
        /// </summary>
        private void HandleNewClientDevice(ClientDevice clientDevice)
        {
            // Bail if this is not ours!
            if(!model.isOwnedLocallySelf) 
                return;
            
            // Else: Apply new value.
            if (debugging)
                Debug.Log($"Setting new clientDevice: {clientDevice}".Green().StartWithFrom(GetType()), this);
            
            model.clientDeviceType = clientDevice;
        }

        /// <summary>
        /// Applies a new <see cref="ClientPhysicalLocation"/> to the class.
        /// </summary>
        private void HandleNewPhysicalLocation(ClientPhysicalLocation clientPhysicalLocation)
        {
            // Bail if this is not ours!
            if(!model.isOwnedLocallySelf) 
                return;
            
            // Else: Apply new value.
            if (debugging)
                Debug.Log($"Setting new clientPhysicalLocation: {clientPhysicalLocation}".Green().StartWithFrom(GetType()), this);
            
            model.physicalLocation = clientPhysicalLocation;
        }

        #endregion

        #region Event Tunnles
        
        public delegate void PropertyChangedHandler<in T>(T value);
        public event PropertyChangedHandler<string> UserNameDidChange;
        public event PropertyChangedHandler<ViewR.StatusManagement.ClientPhysicalLocation> PhysicalLocationDidChange;
        public event PropertyChangedHandler<ViewR.StatusManagement.ClientDevice> ClientDeviceTypeDidChange;
        public event PropertyChangedHandler<int> UserIDDidChange;
        public event PropertyChangedHandler<int> UserRoleDidChange;
        public event PropertyChangedHandler<bool> MegaphonePermissionDidChange;

        private void InvokeUserNameDidChange(string value) => UserNameDidChange?.Invoke(value);
        private void InvokePhysicalLocationDidChange(ClientPhysicalLocation value) => PhysicalLocationDidChange?.Invoke(value);
        private void InvokeClientDeviceTypeDidChange(ClientDevice value) => ClientDeviceTypeDidChange?.Invoke(value);
        private void InvokeUserIDDidChange(int value) => UserIDDidChange?.Invoke(value);
        private void InvokeUserRoleDidChange(int value) => UserRoleDidChange?.Invoke(value);
        private void InvokeMegaphonePermissionDidChange(bool value) => MegaphonePermissionDidChange?.Invoke(value);

        #endregion
    }
}