using Normal.Realtime;
using Oculus.Interaction;
using Pixelplacement;
using SurgeExtensions;
using UnityEngine;
using UnityEngine.Events;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Utils.ObjectDuplication
{
    /// <summary>
    /// Receives queries to spawn new game objects from <see cref="DuplicationSender"/>. Only instantiates them, if free!
    /// + Tween animation on top.
    /// Events for sound.
    /// </summary>
    public class DuplicationReceiver : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent enteredBox;

        [SerializeField]
        private UnityEvent leftBox;

        [SerializeField]
        private UnityEvent spawnedSomething;

        [SerializeField]
        private UnityEvent errorCouldNotSpawnedSomething;

        [SerializeField]
        private TweenConfig tweenConfigScale;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        private bool _isEmpty = true;


        /// <summary>
        /// Lets the box know its occupied and fires the event.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.tag.Equals(DuplicationSender.SEARCHED_TAG))
                return;

            if (debugging)
                Debug.Log($"Collision with {other.gameObject.name}".StartWithFrom(GetType()));

            _isEmpty = false;

            enteredBox?.Invoke();
        }

        /// <summary>
        /// Frees the box again and fires the event.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (!other.tag.Equals(DuplicationSender.SEARCHED_TAG))
                return;

            if (debugging)
                Debug.Log($"Left Collision Area: {other.gameObject.name}".StartWithFrom(GetType()));

            _isEmpty = true;

            leftBox?.Invoke();
        }

        /// <summary>
        /// Queries whether the box is currently free and can take a new item.
        /// </summary>
        public void QueryNewInstance(GameObject originalObject)
        {
            if (!_isEmpty)
            {
                if (debugging)
                    Debug.Log("Not empty. Cant spawn.".StartWithFrom(GetType()));
                errorCouldNotSpawnedSomething?.Invoke();
                return;
            }

            // Else: Spawn!
            NetworkedInstantiation(originalObject);
            
            spawnedSomething?.Invoke();
        }


        /// <summary>
        /// Actually does the instantiation and starts the tweening
        /// </summary>
        private void NetworkedInstantiation(GameObject originalObject)
        {
            // Get the Prefab name because ... Normcore...
            var duplicatableObject = originalObject.GetComponent<DuplicatableObject>();
            var prefabName = duplicatableObject.GetPrefabName();
            var parentWithRealtimeTransform = duplicatableObject.GetParentWithRealtimeTransform();

            // Settings
            var realtimeOptions = new Realtime.InstantiateOptions
            {
                ownedByClient = false,
                destroyWhenLastClientLeaves = false
            };

            // Realtime Instantiate
            var newObject = Realtime.Instantiate(prefabName, this.transform.position, parentWithRealtimeTransform.rotation,
                realtimeOptions);

            // Modify Transform
            var newRealtimeTransform = newObject.GetComponent<RealtimeTransform>();
            newRealtimeTransform.RequestOwnership();
            var newObjectTransform = newObject.transform;
            newObjectTransform.localScale = Vector3.zero;

            // Tween!
            Pixelplacement.Tween.LocalScale(newObjectTransform,
                Vector3.zero,
                parentWithRealtimeTransform.localScale,
                tweenConfigScale.Duration,
                tweenConfigScale.Delay,
                tweenConfigScale.AnimationCurve,
                tweenConfigScale.loopType,
                // Ensure we release the ownership
                completeCallback: () => ReleaseOwnership(newRealtimeTransform),
                obeyTimescale: tweenConfigScale.obeyTimescale);
        }

        /// <summary>
        /// Ensure we release the ownership once we're done tweening.
        /// Also ensures that we don't release it if we have already picked it up, or if it is no longer ours!
        /// </summary>
        private void ReleaseOwnership(RealtimeTransform realtimeTransform)
        {
            // Bail if no longer ours.
            if (!realtimeTransform.isOwnedLocallyInHierarchy)
            {
                if (debugging)
                    Debug.Log($"No longer ours. Wont reset ownership on {realtimeTransform.gameObject.name}.".StartWithFrom(GetType()));
                return;
            }
            
            //! Ensure we don't reset if we already picked it up!
            // Get all "interactables" on the object
            var interactables = realtimeTransform.GetComponents<IInteractable>();

            // Check if its already been picked up, and if yes, bail.
            var currentlyInteractedWith = false;
            foreach (var interactable in interactables)
            {
                currentlyInteractedWith = interactable.State == InteractableState.Select || currentlyInteractedWith;
            }
            if (currentlyInteractedWith)
            {
                if (debugging)
                    Debug.Log($"Already interacted with. Wont reset ownership on {realtimeTransform.gameObject.name}.".StartWithFrom(GetType()));
                return;
            }
            
            if (debugging)
                Debug.Log($"Releasing ownership on {realtimeTransform.gameObject.name}.".StartWithFrom(GetType()));
            
            // Reset ownership
            realtimeTransform.ClearOwnership();
        }
    }
}