using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;

namespace ViewR.Core.UI.FloatingUI.Follower
{
    /// <summary>
    /// Looks at the target.
    /// </summary>
    public class LookAtFollower : MonoBehaviour
    {
        public enum MultiUIGroup
        {
            None,
            MainUI,
            NotificationsUI
        }
        
        [SerializeField]
        private MultiUIGroup belongingToGroup;
        
        [Help("Enabled by the ToggleFollower class.\nConfigured by TargetSetter class.")]
        [ReadOnly]
        public Transform target;

        [ReadOnly]
        public bool iAmFirstItem = true;
        
        [ReadOnly]
        public TargetSetter definingTargetSetter;

        private Transform _firstItemTransform;

        public delegate void NewFirstItem(LookAtFollower newFirstItem, MultiUIGroup value);
        public static event NewFirstItem ReceivedNewFirstItem;

        private void OnEnable()
        {
            if (belongingToGroup == MultiUIGroup.None)
                // No multi UI reactions for this group. Bail.
                return;
            ReceivedNewFirstItem += HandleNewFirstItems;
        }

        private void OnDisable()
        {
            if (belongingToGroup == MultiUIGroup.None)
                // No multi UI reactions for this group. Bail.
                return;
            ReceivedNewFirstItem -= HandleNewFirstItems;
        }

        private void OnDestroy()
        {
            // Ensure array is up to date
            definingTargetSetter.RequestRemove(null, this);
        }

        private void Update()
        {
            // transform.LookAt(target.position);
            if (iAmFirstItem)
            {
                // If first: great, follow target
                var thisTransform = transform;
                thisTransform.forward = (target.transform.position - thisTransform.position) * -1;
            }
            else
                // If not first, mimic orientation of first!
                transform.rotation = _firstItemTransform.rotation;
        }

        public void MakeMeFirstItem()
        {
            InvokeReceivedNewFirstItem(this);
            
            // Ensure we run the logic anyway.
            // if (this.enabled == false)
                HandleNewFirstItems(this, belongingToGroup);
        }

        private void HandleNewFirstItems(LookAtFollower newFirstItem, MultiUIGroup groupOfReceivedFirst)
        {
            if (belongingToGroup != groupOfReceivedFirst)
                // Not relevant to us. Bail.
                return;
            
            if (belongingToGroup == MultiUIGroup.None)
                // No multi UI reactions for this group. Bail.
                return;
            
            iAmFirstItem = newFirstItem == this;
            _firstItemTransform = newFirstItem.transform;
        }
        
        private void InvokeReceivedNewFirstItem(LookAtFollower newFirstItem)
        {
            ReceivedNewFirstItem?.Invoke(newFirstItem, this.belongingToGroup);
        }
    }
}