using UnityEngine;
using ViewR.Core.UI.FloatingUI.Follower;

namespace ViewR.Core.OVR.UX.MultiUI
{
    public class MultiUIMakeMeActive : MonoBehaviour
    {
        [SerializeField]
        private MultiUIManager multiUIManager;
        [SerializeField]
        private TargetFollower targetFollower;
        [SerializeField]
        private ToggleFollower toggleFollower;

        /// <summary>
        /// Unpins the object and makes it active.
        /// </summary>
        public void MakeMeActive()
        {
            toggleFollower.EnableFollowing(true);
            
            multiUIManager.MakeMeActive(targetFollower);
        }
    }
}