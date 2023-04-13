using UnityEngine;

namespace ViewR.Core.UI.FloatingUI.Follower
{
    /// <summary>
    /// Essentially only restores the <see cref="TargetFollower.ArrivedAtTargetFirstTime"/> value on disable, which should only be called when the GameObject is being disabled.
    /// We can't do this within <see cref="ToggleFollower"/>, as it gets disabled for performance reasons when not running.
    /// </summary>
    [DisallowMultipleComponent]
    public class RestoreDefaultTargetFollowerUponDisable : MonoBehaviour
    {
        private TargetFollower _targetFollower;

        // Ensure this is active
        private void Awake()
        {
            if (!enabled)
                enabled = true;
        }

        // The only task of this class
        private void OnDisable()
        {
            if (!_targetFollower)
                _targetFollower = GetComponent<TargetFollower>();
            
            _targetFollower.ArrivedAtTargetFirstTime = false;
        }
    }
}