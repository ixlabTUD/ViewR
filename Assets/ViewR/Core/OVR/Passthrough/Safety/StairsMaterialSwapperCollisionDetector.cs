using UnityEngine;

namespace ViewR.Core.OVR.Passthrough.Safety
{
    /// <summary>
    /// This notifies <see cref="stairsMaterialSwapperWarning"/> <see cref="OnTriggerEnter"/> and <see cref="OnTriggerExit"/> of the users head.
    /// This way, we can have multiple colliders to detect changes for <see cref="StairsMaterialSwapperWarning"/>.
    /// </summary>
    public class StairsMaterialSwapperCollisionDetector : MonoBehaviour
    {
        [SerializeField]
        private StairsMaterialSwapperWarning stairsMaterialSwapperWarning;
    
        private void OnTriggerEnter(Collider other)
        {
            if(!other.tag.Equals("OVRHead"))
                return;

            stairsMaterialSwapperWarning.RegisterCollisionEnter();
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.tag.Equals("OVRHead"))
                return;

            stairsMaterialSwapperWarning.RegisterCollisionExit();
        }
        
    }
}