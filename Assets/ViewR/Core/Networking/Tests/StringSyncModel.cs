using Normal.Realtime;
using Normal.Realtime.Serialization;
using UnityEngine;

namespace ViewR.Core.Networking.Tests
{
    [RealtimeModel]
    public partial class StringSyncModel
    {
        [RealtimeProperty(1, true, true)]
        private string _aVeryLongString;
    }
}

/* ----- Begin Normal Autogenerated Code ----- */
namespace ViewR.Core.Networking.Tests {
    public partial class StringSyncModel : RealtimeModel {
        public string aVeryLongString {
            get {
                return _aVeryLongStringProperty.value;
            }
            set {
                if (_aVeryLongStringProperty.value == value) return;
                _aVeryLongStringProperty.value = value;
                InvalidateReliableLength();
                FireAVeryLongStringDidChange(value);
            }
        }
        
        public delegate void PropertyChangedHandler<in T>(StringSyncModel model, T value);
        public event PropertyChangedHandler<string> aVeryLongStringDidChange;
        
        public enum PropertyID : uint {
            AVeryLongString = 1,
        }
        
        #region Properties
        
        private ReliableProperty<string> _aVeryLongStringProperty;
        
        #endregion
        
        public StringSyncModel() : base(null) {
            _aVeryLongStringProperty = new ReliableProperty<string>(1, _aVeryLongString);
        }
        
        protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
            _aVeryLongStringProperty.UnsubscribeCallback();
        }
        
        private void FireAVeryLongStringDidChange(string value) {
            try {
                aVeryLongStringDidChange?.Invoke(this, value);
            } catch (System.Exception exception) {
                UnityEngine.Debug.LogException(exception);
            }
        }
        
        protected override int WriteLength(StreamContext context) {
            var length = 0;
            length += _aVeryLongStringProperty.WriteLength(context);
            return length;
        }
        
        protected override void Write(WriteStream stream, StreamContext context) {
            var writes = false;
            writes |= _aVeryLongStringProperty.Write(stream, context);
            if (writes) InvalidateContextLength(context);
        }
        
        protected override void Read(ReadStream stream, StreamContext context) {
            var anyPropertiesChanged = false;
            while (stream.ReadNextPropertyID(out uint propertyID)) {
                var changed = false;
                switch (propertyID) {
                    case (uint) PropertyID.AVeryLongString: {
                        changed = _aVeryLongStringProperty.Read(stream, context);
                        if (changed) FireAVeryLongStringDidChange(aVeryLongString);
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
            _aVeryLongString = aVeryLongString;
        }
        
    }
}
/* ----- End Normal Autogenerated Code ----- */
