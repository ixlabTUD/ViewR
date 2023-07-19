using UnityEngine;
using UnityEngine.Events;

namespace ViewR.HelpersLib.Universals.Events
{
    /// <summary>
    /// A simple class that will call the given events whenever this component gets changed - that is that it "awakes, starts, gets enabled or disabled or destroyed.
    /// </summary>
    public class StandardEvents : MonoBehaviour
    {
        public UnityEvent callOnAwake;
        public UnityEvent callOnStart;
        public UnityEvent callOnEnable;
        public UnityEvent callOnDisable;
        public UnityEvent callOnDestroy;

        private void Awake()
        {
            callOnAwake?.Invoke();
        }

        private void Start()
        {
            callOnStart?.Invoke();
        }

        private void OnEnable()
        {
            callOnEnable?.Invoke();
        }

        private void OnDisable()
        {
            callOnDisable?.Invoke();
        }

        private void OnDestroy()
        {
            callOnDestroy?.Invoke();
        }
    }
}