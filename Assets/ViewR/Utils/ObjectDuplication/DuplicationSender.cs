using Normal.Realtime;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Events;
using ViewR.HelpersLib.Extensions.General;
using ViewR.Managers;

namespace ViewR.Utils.ObjectDuplication
{
    /// <summary>
    /// Sends request to spawn new object to the <see cref="DuplicationReceiver"/>.
    /// Events for sound.
    /// </summary>
    public class DuplicationSender : MonoBehaviour
    {
        [SerializeField]
        private DuplicationReceiver duplicationReceiver;

        [SerializeField]
        private UnityEvent enteredBox;

        [SerializeField]
        private UnityEvent leftBox;
        
        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        public const string SEARCHED_TAG = "DuplicatableInteractable";
    
        /// <summary>
        /// Registers collision, invokes event and queries instantiation.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if(!other.tag.Equals(SEARCHED_TAG))
                return;
            
            if (debugging)
                Debug.Log($"Collision with {other.gameObject.name}".StartWithFrom(GetType()));
            
            enteredBox?.Invoke();

            duplicationReceiver.QueryNewInstance(other.gameObject);
        }

        /// <summary>
        /// Not much yet. Fires the event.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if(!other.tag.Equals(SEARCHED_TAG))
                return;
            
            if (debugging)
                Debug.Log($"Left Collision Area: {other.gameObject.name}".StartWithFrom(GetType()));
            
            leftBox?.Invoke();
        }

    }
}
