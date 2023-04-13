using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility;
using ViewR.Managers;

public class EnablePassthroughDropShadow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VirtualEnvironmentVisibility.VirtualEnvironmentVisibilityDidChange += enableShadow;
        
    }

    private void OnEnable()
    {
        ApplyNewVisibleValue(VirtualEnvironmentVisibility.Visible);
    }

    void enableShadow(bool previousValue, bool newVisibleValue)
    {
        ApplyNewVisibleValue(newVisibleValue);
    }
    
    private void ApplyNewVisibleValue(bool newVisibleValue)
    {
        switch (newVisibleValue)
        {
            case true:
                BecameVirtual();
                break;
            case false:
                BecamePassthrough();
                break;
        }
    }
    
    public void BecamePassthrough()
    {

        GetComponent<Renderer>().enabled = true;
    }

    public void BecameVirtual()
    {

        GetComponent<Renderer>().enabled = false;
    }
}
