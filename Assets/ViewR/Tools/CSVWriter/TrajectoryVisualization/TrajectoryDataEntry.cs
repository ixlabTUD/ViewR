using UnityEngine;
using ViewR.Tools.CSVWriter.Mode;

namespace ViewR.Tools.CSVWriter.TrajectoryVisualization
{
    /// <summary>
    /// A class that stores data entries of each trajectory point.
    /// </summary>
    [System.Serializable]
    public class TrajectoryDataEntry
    {
        //! Note that formatting and naming are due to format in the CSV file.
        public int Count;
        public string ID;
        public int Device;
        public int TrackingMode;
        public double Time;
        public float PositionX, PositionY, PositionZ;
        public float QuatW, QuatX, QuatY, QuatZ;
        
        private Vector3 _position = new Vector3(-999, -999, -999);
        private Quaternion _rotation = new Quaternion(-999, -999, -999, -999);
        private bool _initialized;

        public Vector3 Position
        {
            get
            {
                if (!_initialized)
                    Initialize();

                return _position;
            }
            private set => _position = value;
        }

        public Quaternion Rotation
        {
            get
            {
                if (!_initialized)
                    Initialize();

                return _rotation;
            }
            private set => _rotation = value;
        }
        
        public OVRPlugin.SystemHeadset GetDevice() => (OVRPlugin.SystemHeadset)Device;

        public TrackingMode GetTrackingMode() => (TrackingMode) TrackingMode;
        

        private void Initialize(bool forceOverwrite = false)
        {
            // Bail if appropriate.
            if (_initialized && ! forceOverwrite)
                return;

            // Set values
            Position = new Vector3(PositionX, PositionY, PositionZ);
            Rotation = new Quaternion(w: QuatW, x: QuatX, y: QuatY, z: QuatZ);
            
            // Set flag
            _initialized = true;
        }
        
        
        #region Constructors for convenience

        public TrajectoryDataEntry(int count,
            string id,
            // int device,
            // int trackingMode,
            OVRPlugin.SystemHeadset device,
            TrackingMode trackingMode,
            double time,
            float positionX,
            float positionY,
            float positionZ,
            float quatW,
            float quatX,
            float quatY,
            float quatZ)
        {
            this.Count = count;
            this.ID = id;
            this.Device = (int)device;
            this.TrackingMode = (int)trackingMode;
            this.Time = time;
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.PositionZ = positionZ;
            this.QuatW = quatW;
            this.QuatX = quatX;
            this.QuatY = quatY;
            this.QuatZ = quatZ;

            this.Position = new Vector3(positionX, positionY, positionZ);
            this.Rotation = new Quaternion(w: quatW, x: quatX, y: quatY, z: quatZ);
        }
        
        public TrajectoryDataEntry(int count,
            string id,
            // int device,
            // int trackingMode,
            OVRPlugin.SystemHeadset device,
            TrackingMode trackingMode,
            double time,
            Vector3 position,
            Quaternion rotation)
        {
            this.Count = count;
            this.ID = id;
            this.Device = (int)device;
            this.TrackingMode = (int)trackingMode;
            this.Time = time;
            this.PositionX = position.x;
            this.PositionY = position.y;
            this.PositionZ = position.z;
            this.QuatW = rotation.w;
            this.QuatX = rotation.x;
            this.QuatY = rotation.y;
            this.QuatZ = rotation.z;

            this.Position = new Vector3(PositionX, PositionY, PositionZ);
            this.Rotation = new Quaternion(w: QuatW, x: QuatX, y: QuatY, z: QuatZ);
        }

        #endregion
    }
}