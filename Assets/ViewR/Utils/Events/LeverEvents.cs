using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DIVREX.Lever.Logic
{
    public class LeverEvents : MonoBehaviour
    {
        [SerializeField] 
        private Collider settingOff;
        [SerializeField] 
        private Collider settingOn;

        [SerializeField] 
        private UnityEvent turnedOff;
        [SerializeField] 
        private UnityEvent turnedOn;

        private void OnTriggerEnter(Collider other)
        {
            if (other == settingOff)
                turnedOff?.Invoke();
            if (other == settingOn)
                turnedOn?.Invoke();
        }
    }
}
