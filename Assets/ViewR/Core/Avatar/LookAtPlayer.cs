using System.Linq;
using Normal.Realtime;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.Managers;

namespace ViewR.Core.Avatar
{
    public class LookAtPlayer : MonoBehaviour
    {
        public GameObject playerHead;
        public GameObject lEyePivot;
        public GameObject rEyePivot;
        public float maxOutViewingAngle = 60f;

        [Help("This should be true in most cases. The alternative is not as performant. It does require a reference to an NetworkManager.")]
        public bool useManagers = true;

        private Realtime _realtime;
        private RealtimeAvatarManager _realtimeAvatarManager;
        private float _angleDifference;
        private bool _initialized;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize(bool overwriteInit = false)
        {
            if (_initialized && !overwriteInit)
                return;

            // Get the Realtime component on this game object
            _realtime = GetComponent<Realtime>();
            if (useManagers)
                _realtimeAvatarManager = NetworkManager.Instance.RealtimeAvatarManager;

            _initialized = true;
        }

        private void Update()
        {
            // Finds head closest to Player within a Thrustum spanned by MaxOutViewingAngle
            var closest = FindClosestHead();

            // If there's no one to look at
            if (closest == null)
            {
                lEyePivot.transform.rotation = playerHead.transform.rotation;
                rEyePivot.transform.rotation = playerHead.transform.rotation;
            }
            else
            {
                lEyePivot.transform.LookAt(closest.transform);
                rEyePivot.transform.LookAt(closest.transform);
            }
        }

        private GameObject FindClosestHead()
        {
            // Ensure reference:
            if (useManagers && !_realtimeAvatarManager)
            {
                if (NetworkManager.IsInstanceRegistered)
                    _realtimeAvatarManager = NetworkManager.Instance.RealtimeAvatarManager;
            }
            
            //! Inefficient method querying FindGameObjectsWithTag every update! Better implementation below!
            // This is kept entirely for porting only and should not be used.
            if (!useManagers || !NetworkManager.IsInstanceRegistered)
            {
                GameObject[] gos;
                gos = GameObject.FindGameObjectsWithTag("AvatarHead");
                GameObject closest = null;
                var distance = Mathf.Infinity;
                var position = playerHead.transform.position;
                foreach (var go in gos)
                {
                    var playerHeadToGo = go.transform.position - position;

                    _angleDifference = Mathf.Abs(Vector3.Angle(playerHeadToGo, playerHead.transform.forward));
                    if (_angleDifference < maxOutViewingAngle)
                    {
                        var curDistance = playerHeadToGo.sqrMagnitude;
                        if (curDistance < distance && Mathf.Abs(curDistance) > 0.05f)
                        {
                            closest = go;
                            distance = curDistance;
                        }
                    }
                }

                return closest;
            }
            // More efficient way of accessing it:
            else
            {
                var avatars = _realtimeAvatarManager.avatars.Values.ToArray();

                GameObject closest = null;
                var closestDistance = Mathf.Infinity;
                var thisAvatarHeadPosition = playerHead.transform.position;
                
                foreach (var avatar in avatars)
                {
                    var otherAvatar = avatar.gameObject;
                    var playerHeadToOtherAvatar = otherAvatar.transform.position - thisAvatarHeadPosition;

                    // Check angle:
                    _angleDifference = Mathf.Abs(Vector3.Angle(playerHeadToOtherAvatar, playerHead.transform.forward));
                    if (_angleDifference < maxOutViewingAngle)
                    {
                        // Check distance:
                        var curDistance = playerHeadToOtherAvatar.sqrMagnitude;
                        if (curDistance < closestDistance && Mathf.Abs(curDistance) > 0.06f)
                        {
                            closest = otherAvatar;
                            closestDistance = curDistance;
                        }
                    }
                }

                return closest;
            }
        }
    }
}