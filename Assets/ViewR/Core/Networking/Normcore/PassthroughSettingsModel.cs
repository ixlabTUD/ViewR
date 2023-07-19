using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using UnityEngine;
using ViewR.StatusManagement.States;

namespace ViewR.Core.Networking.Normcore
{
    [RealtimeModel]
    public partial class PassthroughSettingsModel
    {
        [RealtimeProperty(1, true, true)]
        private int _passthroughLevel;

        [RealtimeProperty(2, true, true)]
        private float _passthroughOverlay;

        [RealtimeProperty(3, true, true)]
        private float _edgeFilter;

        [RealtimeProperty(4, true, true)]
        private UserRepresentationType _avatarMode;

        [RealtimeProperty(5, true, true)]
        private float _passthroughOpacitySpace;
        
        [RealtimeProperty(6, true, true)]
        private float _passthroughOpacitySelective;

    }
}

/* ----- Begin Normal Autogenerated Code ----- */
namespace ViewR.Core.Networking.Normcore {
    public partial class PassthroughSettingsModel : RealtimeModel {
        public int passthroughLevel {
            get {
                return _passthroughLevelProperty.value;
            }
            set {
                if (_passthroughLevelProperty.value == value) return;
                _passthroughLevelProperty.value = value;
                InvalidateReliableLength();
                FirePassthroughLevelDidChange(value);
            }
        }
        
        public float passthroughOverlay {
            get {
                return _passthroughOverlayProperty.value;
            }
            set {
                if (_passthroughOverlayProperty.value == value) return;
                _passthroughOverlayProperty.value = value;
                InvalidateReliableLength();
                FirePassthroughOverlayDidChange(value);
            }
        }
        
        public float edgeFilter {
            get {
                return _edgeFilterProperty.value;
            }
            set {
                if (_edgeFilterProperty.value == value) return;
                _edgeFilterProperty.value = value;
                InvalidateReliableLength();
                FireEdgeFilterDidChange(value);
            }
        }
        
        public ViewR.StatusManagement.States.UserRepresentationType avatarMode {
            get {
                return (ViewR.StatusManagement.States.UserRepresentationType) _avatarModeProperty.value;
            }
            set {
                if (_avatarModeProperty.value == (uint) value) return;
                _avatarModeProperty.value = (uint) value;
                InvalidateReliableLength();
                FireAvatarModeDidChange(value);
            }
        }
        
        public float passthroughOpacitySpace {
            get {
                return _passthroughOpacitySpaceProperty.value;
            }
            set {
                if (_passthroughOpacitySpaceProperty.value == value) return;
                _passthroughOpacitySpaceProperty.value = value;
                InvalidateReliableLength();
                FirePassthroughOpacitySpaceDidChange(value);
            }
        }
        
        public float passthroughOpacitySelective {
            get {
                return _passthroughOpacitySelectiveProperty.value;
            }
            set {
                if (_passthroughOpacitySelectiveProperty.value == value) return;
                _passthroughOpacitySelectiveProperty.value = value;
                InvalidateReliableLength();
                FirePassthroughOpacitySelectiveDidChange(value);
            }
        }
        
        public delegate void PropertyChangedHandler<in T>(PassthroughSettingsModel model, T value);
        public event PropertyChangedHandler<int> passthroughLevelDidChange;
        public event PropertyChangedHandler<float> passthroughOverlayDidChange;
        public event PropertyChangedHandler<float> edgeFilterDidChange;
        public event PropertyChangedHandler<ViewR.StatusManagement.States.UserRepresentationType> avatarModeDidChange;
        public event PropertyChangedHandler<float> passthroughOpacitySpaceDidChange;
        public event PropertyChangedHandler<float> passthroughOpacitySelectiveDidChange;
        
        public enum PropertyID : uint {
            PassthroughLevel = 1,
            PassthroughOverlay = 2,
            EdgeFilter = 3,
            AvatarMode = 4,
            PassthroughOpacitySpace = 5,
            PassthroughOpacitySelective = 6,
        }
        
        #region Properties
        
        private ReliableProperty<int> _passthroughLevelProperty;
        
        private ReliableProperty<float> _passthroughOverlayProperty;
        
        private ReliableProperty<float> _edgeFilterProperty;
        
        private ReliableProperty<uint> _avatarModeProperty;
        
        private ReliableProperty<float> _passthroughOpacitySpaceProperty;
        
        private ReliableProperty<float> _passthroughOpacitySelectiveProperty;
        
        #endregion
        
        public PassthroughSettingsModel() : base(null) {
            _passthroughLevelProperty = new ReliableProperty<int>(1, _passthroughLevel);
            _passthroughOverlayProperty = new ReliableProperty<float>(2, _passthroughOverlay);
            _edgeFilterProperty = new ReliableProperty<float>(3, _edgeFilter);
            _avatarModeProperty = new ReliableProperty<uint>(4, (uint) _avatarMode);
            _passthroughOpacitySpaceProperty = new ReliableProperty<float>(5, _passthroughOpacitySpace);
            _passthroughOpacitySelectiveProperty = new ReliableProperty<float>(6, _passthroughOpacitySelective);
        }
        
        protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
            _passthroughLevelProperty.UnsubscribeCallback();
            _passthroughOverlayProperty.UnsubscribeCallback();
            _edgeFilterProperty.UnsubscribeCallback();
            _avatarModeProperty.UnsubscribeCallback();
            _passthroughOpacitySpaceProperty.UnsubscribeCallback();
            _passthroughOpacitySelectiveProperty.UnsubscribeCallback();
        }
        
        private void FirePassthroughLevelDidChange(int value) {
            try {
                passthroughLevelDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        private void FirePassthroughOverlayDidChange(float value) {
            try {
                passthroughOverlayDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        private void FireEdgeFilterDidChange(float value) {
            try {
                edgeFilterDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        private void FireAvatarModeDidChange(ViewR.StatusManagement.States.UserRepresentationType value) {
            try {
                avatarModeDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        private void FirePassthroughOpacitySpaceDidChange(float value) {
            try {
                passthroughOpacitySpaceDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        private void FirePassthroughOpacitySelectiveDidChange(float value) {
            try {
                passthroughOpacitySelectiveDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        protected override int WriteLength(StreamContext context) {
            var length = 0;
            length += _passthroughLevelProperty.WriteLength(context);
            length += _passthroughOverlayProperty.WriteLength(context);
            length += _edgeFilterProperty.WriteLength(context);
            length += _avatarModeProperty.WriteLength(context);
            length += _passthroughOpacitySpaceProperty.WriteLength(context);
            length += _passthroughOpacitySelectiveProperty.WriteLength(context);
            return length;
        }
        
        protected override void Write(WriteStream stream, StreamContext context) {
            var writes = false;
            writes |= _passthroughLevelProperty.Write(stream, context);
            writes |= _passthroughOverlayProperty.Write(stream, context);
            writes |= _edgeFilterProperty.Write(stream, context);
            writes |= _avatarModeProperty.Write(stream, context);
            writes |= _passthroughOpacitySpaceProperty.Write(stream, context);
            writes |= _passthroughOpacitySelectiveProperty.Write(stream, context);
            if (writes) InvalidateContextLength(context);
        }
        
        protected override void Read(ReadStream stream, StreamContext context) {
            var anyPropertiesChanged = false;
            while (stream.ReadNextPropertyID(out uint propertyID)) {
                var changed = false;
                switch (propertyID) {
                    case (uint) PropertyID.PassthroughLevel: {
                        changed = _passthroughLevelProperty.Read(stream, context);
                        if (changed) FirePassthroughLevelDidChange(passthroughLevel);
                        break;
                    }
                    case (uint) PropertyID.PassthroughOverlay: {
                        changed = _passthroughOverlayProperty.Read(stream, context);
                        if (changed) FirePassthroughOverlayDidChange(passthroughOverlay);
                        break;
                    }
                    case (uint) PropertyID.EdgeFilter: {
                        changed = _edgeFilterProperty.Read(stream, context);
                        if (changed) FireEdgeFilterDidChange(edgeFilter);
                        break;
                    }
                    case (uint) PropertyID.AvatarMode: {
                        changed = _avatarModeProperty.Read(stream, context);
                        if (changed) FireAvatarModeDidChange(avatarMode);
                        break;
                    }
                    case (uint) PropertyID.PassthroughOpacitySpace: {
                        changed = _passthroughOpacitySpaceProperty.Read(stream, context);
                        if (changed) FirePassthroughOpacitySpaceDidChange(passthroughOpacitySpace);
                        break;
                    }
                    case (uint) PropertyID.PassthroughOpacitySelective: {
                        changed = _passthroughOpacitySelectiveProperty.Read(stream, context);
                        if (changed) FirePassthroughOpacitySelectiveDidChange(passthroughOpacitySelective);
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
            _passthroughLevel = passthroughLevel;
            _passthroughOverlay = passthroughOverlay;
            _edgeFilter = edgeFilter;
            _avatarMode = avatarMode;
            _passthroughOpacitySpace = passthroughOpacitySpace;
            _passthroughOpacitySelective = passthroughOpacitySelective;
        }
        
    }
}
/* ----- End Normal Autogenerated Code ----- */
