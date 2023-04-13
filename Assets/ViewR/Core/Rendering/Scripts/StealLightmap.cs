using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility;

[ExecuteInEditMode]
public class StealLightmap : MonoBehaviour {

    private MeshRenderer currentRenderer;
    public MeshRenderer lightmappedObject;


    private void OnEnable()
    {
        Awake();
    }

    private void Awake()
    {
        currentRenderer = gameObject.GetComponent<MeshRenderer>();
        currentRenderer.enabled = false;
        
        //lightmappedObject = GetComponent<MeshRenderer>();
        //endererInfoTransfer();
        RendererInfoTransferToTexture();
        
        
        VirtualEnvironmentVisibility.VirtualEnvironmentVisibilityDidChange += HandleVisibilityChanges;
    }

    void Initialize()
    {
        currentRenderer = GetComponent<MeshRenderer>();
    }

#if UNITY_EDITOR
    void OnBecameVisible()
    {
        //RendererInfoTransfer();
        RendererInfoTransferToTexture();
    }
#endif

    void RendererInfoTransfer()
    {
        if(lightmappedObject == null || currentRenderer == null)
            return;

        currentRenderer.lightmapIndex = lightmappedObject.lightmapIndex;
        currentRenderer.lightmapScaleOffset = lightmappedObject.lightmapScaleOffset;
        currentRenderer.realtimeLightmapIndex = lightmappedObject.realtimeLightmapIndex;
        currentRenderer.realtimeLightmapScaleOffset = lightmappedObject.realtimeLightmapScaleOffset;
        currentRenderer.lightProbeUsage = lightmappedObject.lightProbeUsage;
    }
    
    
    private void HandleVisibilityChanges(bool previousValue, bool newVisibleValue)
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
        if (!currentRenderer)
            Initialize();



        currentRenderer.enabled = true;
    }

    public void BecameVirtual()
    {
        if (!currentRenderer)
            Initialize();

        currentRenderer.enabled = false;
    }
    
    void RendererInfoTransferToTexture()
    {
        if(lightmappedObject == null || currentRenderer == null)
            return;

        //currentRenderer.
        //transform.GetComponent<MeshFilter>().sharedMesh.uv2 = lightmappedObject.GetComponent<MeshFilter>().sharedMesh.uv2;
        LightmapData lightmapData = LightmapSettings.lightmaps[lightmappedObject.lightmapIndex];
        Vector4 scaleOffset = lightmappedObject.lightmapScaleOffset;
        
        GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_AlphaTexture", lightmapData.lightmapColor);
        GetComponent<MeshRenderer>().sharedMaterial.SetVector("_ScaleOffset", scaleOffset);

        //Texture2D tex = new Texture2D(lightmappedObject.);
        
        // currentRenderer.lightmapIndex = lightmappedObject.lightmapIndex;
        // currentRenderer.lightmapScaleOffset = lightmappedObject.lightmapScaleOffset;
        // currentRenderer.realtimeLightmapIndex = lightmappedObject.realtimeLightmapIndex;
        // currentRenderer.realtimeLightmapScaleOffset = lightmappedObject.realtimeLightmapScaleOffset;
        // currentRenderer.lightProbeUsage = lightmappedObject.lightProbeUsage;
    }
}
