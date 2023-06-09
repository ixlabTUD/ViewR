using Normal.Realtime;
using Normal.Realtime.Serialization;
using UnityEngine;
using ViewR.StatusManagement;

namespace ViewR.Core.Networking.Normcore.SyncPlayerProperties
{
    [RealtimeModel]
    public partial class SyncedPlayerPropertiesModel
    {
        [RealtimeProperty(1, true, true)]
        private string _userName;
        
        [RealtimeProperty(2, true, true)]
        private ClientPhysicalLocation _physicalLocation;
        
        [RealtimeProperty(3, true, true)]
        private ClientDevice _clientDeviceType;
        
        // Optional / only for future use, as updating these classes is a pain:
        [RealtimeProperty(4, true, true)]
        private int _userID;
        [RealtimeProperty(5, true, true)]
        private int _userRole;
        [RealtimeProperty(6, true, true)]
        private bool _megaphonePermission;
    }
}

/* ----- Begin Normal Autogenerated Code ----- */
namespace ViewR.Core.Networking.Normcore.SyncPlayerProperties {
    public partial class SyncedPlayerPropertiesModel : RealtimeModel {
        public string userName {
            get {
                return _userNameProperty.value;
            }
            set {
                if (_userNameProperty.value == value) return;
                _userNameProperty.value = value;
                InvalidateReliableLength();
                FireUserNameDidChange(value);
            }
        }
        
        public ViewR.StatusManagement.ClientPhysicalLocation physicalLocation {
            get {
                return (ViewR.StatusManagement.ClientPhysicalLocation) _physicalLocationProperty.value;
            }
            set {
                if (_physicalLocationProperty.value == (uint) value) return;
                _physicalLocationProperty.value = (uint) value;
                InvalidateReliableLength();
                FirePhysicalLocationDidChange(value);
            }
        }
        
        public ViewR.StatusManagement.ClientDevice clientDeviceType {
            get {
                return (ViewR.StatusManagement.ClientDevice) _clientDeviceTypeProperty.value;
            }
            set {
                if (_clientDeviceTypeProperty.value == (uint) value) return;
                _clientDeviceTypeProperty.value = (uint) value;
                InvalidateReliableLength();
                FireClientDeviceTypeDidChange(value);
            }
        }
        
        public int userID {
            get {
                return _userIDProperty.value;
            }
            set {
                if (_userIDProperty.value == value) return;
                _userIDProperty.value = value;
                InvalidateReliableLength();
                FireUserIDDidChange(value);
            }
        }
        
        public int userRole {
            get {
                return _userRoleProperty.value;
            }
            set {
                if (_userRoleProperty.value == value) return;
                _userRoleProperty.value = value;
                InvalidateReliableLength();
                FireUserRoleDidChange(value);
            }
        }
        
        public bool megaphonePermission {
            get {
                return _megaphonePermissionProperty.value;
            }
            set {
                if (_megaphonePermissionProperty.value == value) return;
                _megaphonePermissionProperty.value = value;
                InvalidateReliableLength();
                FireMegaphonePermissionDidChange(value);
            }
        }
        
        public delegate void PropertyChangedHandler<in T>(SyncedPlayerPropertiesModel model, T value);
        public event PropertyChangedHandler<string> userNameDidChange;
        public event PropertyChangedHandler<ViewR.StatusManagement.ClientPhysicalLocation> physicalLocationDidChange;
        public event PropertyChangedHandler<ViewR.StatusManagement.ClientDevice> clientDeviceTypeDidChange;
        public event PropertyChangedHandler<int> userIDDidChange;
        public event PropertyChangedHandler<int> userRoleDidChange;
        public event PropertyChangedHandler<bool> megaphonePermissionDidChange;
        
        public enum PropertyID : uint {
            UserName = 1,
            PhysicalLocation = 2,
            ClientDeviceType = 3,
            UserID = 4,
            UserRole = 5,
            MegaphonePermission = 6,
        }
        
        #region Properties
        
        private ReliableProperty<string> _userNameProperty;
        
        private ReliableProperty<uint> _physicalLocationProperty;
        
        private ReliableProperty<uint> _clientDeviceTypeProperty;
        
        private ReliableProperty<int> _userIDProperty;
        
        private ReliableProperty<int> _userRoleProperty;
        
        private ReliableProperty<bool> _megaphonePermissionProperty;
        
        #endregion
        
        public SyncedPlayerPropertiesModel() : base(null) {
            _userNameProperty = new ReliableProperty<string>(1, _userName);
            _physicalLocationProperty = new ReliableProperty<uint>(2, (uint) _physicalLocation);
            _clientDeviceTypeProperty = new ReliableProperty<uint>(3, (uint) _clientDeviceType);
            _userIDProperty = new ReliableProperty<int>(4, _userID);
            _userRoleProperty = new ReliableProperty<int>(5, _userRole);
            _megaphonePermissionProperty = new ReliableProperty<bool>(6, _megaphonePermission);
        }
        
        protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
            _userNameProperty.UnsubscribeCallback();
            _physicalLocationProperty.UnsubscribeCallback();
            _clientDeviceTypeProperty.UnsubscribeCallback();
            _userIDProperty.UnsubscribeCallback();
            _userRoleProperty.UnsubscribeCallback();
            _megaphonePermissionProperty.UnsubscribeCallback();
        }
        
        private void FireUserNameDidChange(string value) {
            try {
                userNameDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        private void FirePhysicalLocationDidChange(ViewR.StatusManagement.ClientPhysicalLocation value) {
            try {
                physicalLocationDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        private void FireClientDeviceTypeDidChange(ViewR.StatusManagement.ClientDevice value) {
            try {
                clientDeviceTypeDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        private void FireUserIDDidChange(int value) {
            try {
                userIDDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        private void FireUserRoleDidChange(int value) {
            try {
                userRoleDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        private void FireMegaphonePermissionDidChange(bool value) {
            try {
                megaphonePermissionDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        protected override int WriteLength(StreamContext context) {
            var length = 0;
            length += _userNameProperty.WriteLength(context);
            length += _physicalLocationProperty.WriteLength(context);
            length += _clientDeviceTypeProperty.WriteLength(context);
            length += _userIDProperty.WriteLength(context);
            length += _userRoleProperty.WriteLength(context);
            length += _megaphonePermissionProperty.WriteLength(context);
            return length;
        }
        
        protected override void Write(WriteStream stream, StreamContext context) {
            var writes = false;
            writes |= _userNameProperty.Write(stream, context);
            writes |= _physicalLocationProperty.Write(stream, context);
            writes |= _clientDeviceTypeProperty.Write(stream, context);
            writes |= _userIDProperty.Write(stream, context);
            writes |= _userRoleProperty.Write(stream, context);
            writes |= _megaphonePermissionProperty.Write(stream, context);
            if (writes) InvalidateContextLength(context);
        }
        
        protected override void Read(ReadStream stream, StreamContext context) {
            var anyPropertiesChanged = false;
            while (stream.ReadNextPropertyID(out uint propertyID)) {
                var changed = false;
                switch (propertyID) {
                    case (uint) PropertyID.UserName: {
                        changed = _userNameProperty.Read(stream, context);
                        if (changed) FireUserNameDidChange(userName);
                        break;
                    }
                    case (uint) PropertyID.PhysicalLocation: {
                        changed = _physicalLocationProperty.Read(stream, context);
                        if (changed) FirePhysicalLocationDidChange(physicalLocation);
                        break;
                    }
                    case (uint) PropertyID.ClientDeviceType: {
                        changed = _clientDeviceTypeProperty.Read(stream, context);
                        if (changed) FireClientDeviceTypeDidChange(clientDeviceType);
                        break;
                    }
                    case (uint) PropertyID.UserID: {
                        changed = _userIDProperty.Read(stream, context);
                        if (changed) FireUserIDDidChange(userID);
                        break;
                    }
                    case (uint) PropertyID.UserRole: {
                        changed = _userRoleProperty.Read(stream, context);
                        if (changed) FireUserRoleDidChange(userRole);
                        break;
                    }
                    case (uint) PropertyID.MegaphonePermission: {
                        changed = _megaphonePermissionProperty.Read(stream, context);
                        if (changed) FireMegaphonePermissionDidChange(megaphonePermission);
                        break;
                    }
                    default: {
                        stream.SkipProperty();
                        break;
                    }
                }
                anyPropertiesChanged |= changed;
            }
            if (anyPropertiesChanged) {
                UpdateBackingFields();
            }
        }
        
        private void UpdateBackingFields() {
            _userName = userName;
            _physicalLocation = physicalLocation;
            _clientDeviceType = clientDeviceType;
            _userID = userID;
            _userRole = userRole;
            _megaphonePermission = megaphonePermission;
        }
        
    }
}
/* ----- End Normal Autogenerated Code ----- */
