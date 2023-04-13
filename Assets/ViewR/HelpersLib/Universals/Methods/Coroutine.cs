using System;
using System.Collections;
using UnityEngine;

public static class CoroutineExtensions
{
    public static IEnumerator StartCallbackAfterSeconds(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }
}
