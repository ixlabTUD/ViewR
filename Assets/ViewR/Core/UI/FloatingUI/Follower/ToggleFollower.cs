using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ViewR.Core.UI.FloatingUI.Follower
{
    /// <summary>
    /// Toggles the <see cref="TargetFollower"/> and <see cref="LookAtFollower"/> classes
    /// </summary>
    public class ToggleFollower : MonoBehaviour
    {
        [FormerlySerializedAs("toggleOnEnable")]
        [SerializeField]
        private bool enableFollowerOnEnable = true;
        [SerializeField]
        private LookAtFollower lookAtFollower;
        [SerializeField]
        private TargetFollower targetFollower;

        [SerializeField]
        private Image pinBar;

        public LookAtFollower LookAtFollower => lookAtFollower;
        public TargetFollower TargetFollower => targetFollower;

        private void OnEnable()
        {
            if(!enableFollowerOnEnable) return;
            EnableFollowing(true);
        }
        
        private void OnDisable()
        {
            if(!enableFollowerOnEnable) return;
            EnableFollowing(false);
        }


        public void EnableFollowing(bool enable)
        {
            lookAtFollower.enabled = targetFollower.enabled = enable;
            pinBar.gameObject.SetActive(!enable);
        }

        public void ToggleFollowing()
        {
            EnableFollowing(!targetFollower.enabled);
        }
        
    }
}
