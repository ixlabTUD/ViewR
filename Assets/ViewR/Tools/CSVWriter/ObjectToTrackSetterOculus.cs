using UnityEngine;

namespace ViewR.Tools.CSVWriter
{
    public class ObjectToTrackSetterOculus : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private Transform quest1ControllerCenterLeft;
        [SerializeField]
        private Transform quest1ControllerCenterRight;
        [SerializeField]
        private Transform quest2ControllerCenterLeft;
        [SerializeField]
        private Transform quest2ControllerCenterRight;
        [SerializeField]
        private Transform questProControllerCenterLeft;
        [SerializeField]
        private Transform questProControllerCenterRight;
        [SerializeField]
        private Transform centerEye;
        
        
        [Header("References")]
        [SerializeField]
        private CsvPoseRecorderBase[] csvRecordPoseOnRequest;

        private void Awake()
        {
            SetTargetsToGivenTransforms();
        }

        public void SetTargetsToGivenTransforms()
        {
            var questModel = OVRPlugin.GetSystemHeadsetType();
            var newTargets = new Transform[3];
            newTargets[0] = centerEye;

            // Populate array correctly
            switch (questModel)
            {
                case OVRPlugin.SystemHeadset.Oculus_Quest:
                case OVRPlugin.SystemHeadset.Oculus_Link_Quest:
                    Debug.Log("Setting new CSV targets to Quest1.", this);
                    newTargets[1] = quest1ControllerCenterLeft;
                    newTargets[2] = quest1ControllerCenterRight;
                    break;
                case OVRPlugin.SystemHeadset.Oculus_Link_Quest_2:
                case OVRPlugin.SystemHeadset.Oculus_Quest_2:
                    Debug.Log("Setting new CSV targets to Quest2.", this);
                    newTargets[1] = quest2ControllerCenterLeft;
                    newTargets[2] = quest2ControllerCenterRight;
                    break;
                case OVRPlugin.SystemHeadset.Meta_Quest_Pro:
                case OVRPlugin.SystemHeadset.Meta_Link_Quest_Pro:
                    Debug.Log("Setting new CSV targets to Quest Pro.", this);
                    newTargets[1] = questProControllerCenterLeft;
                    newTargets[2] = questProControllerCenterRight;
                    break;
                case OVRPlugin.SystemHeadset.None:
                case OVRPlugin.SystemHeadset.Placeholder_11:
                case OVRPlugin.SystemHeadset.Placeholder_12:
                case OVRPlugin.SystemHeadset.Placeholder_13:
                case OVRPlugin.SystemHeadset.Placeholder_14:
                case OVRPlugin.SystemHeadset.Rift_DK1:
                case OVRPlugin.SystemHeadset.Rift_DK2:
                case OVRPlugin.SystemHeadset.Rift_CV1:
                case OVRPlugin.SystemHeadset.Rift_CB:
                case OVRPlugin.SystemHeadset.Rift_S:
                case OVRPlugin.SystemHeadset.PC_Placeholder_4104:
                case OVRPlugin.SystemHeadset.PC_Placeholder_4105:
                case OVRPlugin.SystemHeadset.PC_Placeholder_4106:
                case OVRPlugin.SystemHeadset.PC_Placeholder_4107:
                default:
                    Debug.LogWarning("The system is not configured for your device!" +
                                     "Setting new CSV targets to Quest2 anyway. Results may be inaccurate!", this);
                    break;
            }

            // Apply on all recorders
            foreach (var csvPoseRecorderBase in csvRecordPoseOnRequest)
            {
                csvPoseRecorderBase.SetObjectsToTrack(newTargets);
            }
        }
    }
}
