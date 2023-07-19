using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using ViewR.Core.OVR.Interactions.ForceRelease;
using ViewR.HelpersLib.Extensions.EditorExtensions.ShowAttribute;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Core.Networking.Normcore.Ownership
{
    /// <summary>
    /// Requests ownership for given <see cref="RealtimeTransform"/>.
    /// Clearing ownership can be done in two ways. If <see cref="considerPhysics"/> is true, we will consider the objects rigidbody's velocity and a potential countdown. Else, we will just clear the ownership.
    /// </summary>
    [RequireComponent(typeof(ForceControlInteractables))]
    public class InteractableOwnershipRequester : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IPointable))]
        private MonoBehaviour _pointable;
        private IPointable pointable;
        
        [FormerlySerializedAs("advancedMode")]
        [SerializeField]
        private bool considerPhysics = false;

        [SerializeField, ShowIf(ActionOnConditionFail.DisableInspectorEditing, ConditionOperator.OR, nameof(considerPhysics))]
        private float delayedOwnershipReleaseTime = 0f;

        [SerializeField]
        private RealtimeTransform realtimeTransform;

        [SerializeField, Optional]
        private new Rigidbody rigidbody = null;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        private Vector3 _startPosition = Vector3.zero;
        private Quaternion _startRotation = Quaternion.identity;
        private Vector3 _startScale = Vector3.zero;
        private Coroutine _releaseRoutine;
        private HashSet<int> _pointersCurrentlySelecting;
        private ForceControlInteractables _forceControlInteractables;

        private const float Vector3ZeroMagnitudeThreshold = 0.0001f;

        protected bool Started = false;

        protected virtual void Awake()
        {
            // Cast and get references
            pointable = _pointable as IPointable;
            if(rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();
            // Get realtime if not set:
            if(realtimeTransform == null)
                realtimeTransform = GetComponent<RealtimeTransform>();
            
            _forceControlInteractables = GetComponent<ForceControlInteractables>();

            // Save values.
            var thisTransform = transform;
            _startPosition = thisTransform.localPosition;
            _startRotation = thisTransform.localRotation;
            _startScale = thisTransform.localScale;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref Started);
            Assert.IsNotNull(pointable);
            Assert.IsNotNull(realtimeTransform);
            Assert.IsNotNull(_forceControlInteractables);
            _pointersCurrentlySelecting = new HashSet<int>();
            this.EndStart(ref Started);
        }

        protected virtual void OnEnable()
        {
            if (Started)
            {
                pointable.WhenPointerEventRaised += HandlePointerEventRaised;
            }
        }

        protected virtual void OnDisable()
        {
            if (Started)
            {
                pointable.WhenPointerEventRaised -= HandlePointerEventRaised;
            
                Debug.Log("Un-Requesting Ownership because this was disabled.".StartWithFrom(GetType()), this);
                UnRequest();
            }
        }

        private void HandlePointerEventRaised(PointerEvent evt)
        {
            switch (evt.Type)
            {
                case PointerEventType.Hover:
                case PointerEventType.Move:
                case PointerEventType.Unhover:
                    // Nothing to do!
                    break;
                case PointerEventType.Select:
                    _pointersCurrentlySelecting.Add(evt.Identifier);
                    Request();
                    break;
                case PointerEventType.Cancel:
                case PointerEventType.Unselect:
                    if (_pointersCurrentlySelecting.Contains(evt.Identifier))
                    {
                        // Released!
                        if (_pointersCurrentlySelecting.Count <= 1)
                            // Last pointer was released! Un-request.
                            UnRequest();
                        
                        // Remove from set
                        _pointersCurrentlySelecting.Remove(evt.Identifier);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Only request ownership if the item is unowned in hierarchy!
        /// Otherwise, it will run a <see cref="ForceControlInteractables.ForceRelease()"/>, forcing the client to not hold onto the item, avoiding holding a local copy of the item that will snap to its networked pose.
        /// </summary>
        private void Request()
        {
            // Bail if not online.
            if (!realtimeTransform.realtime.connected)
                return;
            
            // Stop current release routine
            if (_releaseRoutine != null)
                StopCoroutine(_releaseRoutine);
            
            // Bail not unowned.
            // ToDo: Improve for Physics
            if (!realtimeTransform.isUnownedInHierarchy)
            {
                // Force-release if NOT owned by ourselves!
                if (!realtimeTransform.isOwnedLocallyInHierarchy)
                    _forceControlInteractables.ForceRelease();
                return;
            }
            
            if (debugging)
                Debug.Log($"Requesting Ownership on {this.name}!");
            // Actually request
            realtimeTransform.RequestOwnership();
        }

        private void UnRequest()
        {
            // Bail if not online.
            if (!realtimeTransform.realtime.connected)
                return;

            // Bail if not owned by us.
            if (!realtimeTransform.isOwnedLocallyInHierarchy)
                return;
            
            if (debugging)
                Debug.Log($"Clearing Ownership on {this.name}!");
            
            // Clear ownership
            if (!considerPhysics || rigidbody == null)
                realtimeTransform.ClearOwnership();
            else
                _releaseRoutine = StartCoroutine(ClearOwnershipOnceAtRest(delayedOwnershipReleaseTime));
        }

        /// <summary>
        /// Clears the ownership once at rest.
        /// We will let normcore check for the resetting the ownership on sleep - however, this proves unreliable on Standalone.
        /// So, we will wait until the <see cref="rigidbody"/>s velocity is near 0,0,0, and then clear the ownership IF it is still ours.
        /// </summary>
        /// <param name="delayedOwnershipReleaseTime">Time to wait until spinning for velocity == 0.</param>
        private IEnumerator ClearOwnershipOnceAtRest(float delayedOwnershipReleaseTime)
        {
            // Wait to apply velocity
            yield return new WaitForSeconds(.1f);
            
            // Spin while it is still moving.
            while (rigidbody != null && !Vector3NearZero(rigidbody.velocity))
            {
                // Bail if no longer ours
                if (!realtimeTransform.isOwnedLocallyInHierarchy) 
                    yield break;
                
                if (debugging)
                    Debug.Log(rigidbody.velocity);
                
                yield return null;
            }

            yield return new WaitForSeconds(delayedOwnershipReleaseTime);

            if (debugging)
                Debug.Log($"Clearing Ownership on {this.name} DELAYED!");
            
            // Only clear if we are still the owner. Else: Bail
            if (!realtimeTransform.isOwnedLocallyInHierarchy) 
                yield break;
            
            rigidbody.velocity = Vector3.zero;
            realtimeTransform.ClearOwnership();
        }

        private bool Vector3NearZero(Vector3 vector3ToInspect)
        {
            return vector3ToInspect.sqrMagnitude < Vector3ZeroMagnitudeThreshold;
        }

        public void ResetObjectTransform()
        {
            // Ensure we can set run this.
            realtimeTransform.RequestOwnership();

            // Reset Transform
            var thisTransform = transform;
            thisTransform.localPosition = _startPosition;
            thisTransform.localRotation = _startRotation;
            thisTransform.localScale = _startScale;

            // Reset Rigidbody
            if (rigidbody != null)
            {
                rigidbody.angularVelocity = Vector3.zero;
                rigidbody.velocity = Vector3.zero;
            }
        }

        // Convenience
        private void OnValidate()
        {
            if(realtimeTransform == null)
                realtimeTransform = GetComponent<RealtimeTransform>();
        }
    }
}