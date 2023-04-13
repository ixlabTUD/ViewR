using UnityEngine;
using ViewR.HelpersLib.Extensions.General;
using ViewR.HelpersLib.Extensions.JSON;

namespace ViewR.Tools.CSVWriter.TrajectoryVisualization
{
    public class TrajectoryImportHandler : MonoBehaviour
    {
        [SerializeField]
        private string filePathInPersistentFolder;
        [SerializeField]
        private bool readOnEnable = true;
        [SerializeField]
        private bool usePersistentPath = true;
        
        [Header("Optional")]
        [SerializeField]
        private bool storeInPositionsArray = true;

        private TrajectoryDataEntry[] _trajectoryData = {};
        private Vector3[] _positions;

        public Vector3[] Positions
        {
            get
            {
                if (storeInPositionsArray)
                    return _positions;

                // Else: Fetch all positions from array
                var tmpPositions = new Vector3[_trajectoryData.Length];
                for (var i = 0; i < tmpPositions.Length; i++)
                    tmpPositions[i] = _trajectoryData[i].Position;
                return tmpPositions;
            }
            private set => _positions = value;
        }


        // Fetch current list
        private void OnEnable()
        {
            if (readOnEnable)
                ReadData();
        }

        /// <summary>
        /// Read the data
        /// </summary>
        public void ReadData()
        {
            // Read the data
            _trajectoryData = FileHandler.ReadArrayFromJSON<TrajectoryDataEntry>(filePathInPersistentFolder, usePersistentPath: usePersistentPath);
            
            Debug.Log("Success! Loaded Data!".Green(), this);

            // Convenience
            PopulatePositionsArray();
        }

        /// <summary>
        /// For convenience, all positions can be stored as an array outside of <see cref="_trajectoryData"/>
        /// </summary>
        private void PopulatePositionsArray()
        {
            Positions = new Vector3[_trajectoryData.Length];
            for (var i = 0; i < Positions.Length; i++)
            {
                Positions[i] = _trajectoryData[i].Position;
            }
        }
    }
}