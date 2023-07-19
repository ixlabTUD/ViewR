using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvents : MonoBehaviour
{
    public UnityEvent callOnStart;
    public UnityEvent callOnEnable;
    [SerializeField] private float waitTime;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(waitTime);
        
        callOnStart?.Invoke();
    }

    private void OnEnable()
    {
        StartCoroutine(OnEnableDelayed());
    }

    private IEnumerator OnEnableDelayed()
    {
        yield return new WaitForSeconds(waitTime);
        
        callOnEnable?.Invoke();
    }
}
