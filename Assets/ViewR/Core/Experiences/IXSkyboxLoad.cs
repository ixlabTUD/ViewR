using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IXSkyboxLoad : MonoBehaviour
{

    private Material defaultSkyboxMaterial;
    // Start is called before the first frame update
    void Start()
    {
        defaultSkyboxMaterial = RenderSettings.skybox;
    }

    public void loadDeafaultSkybox()
    {
        if (defaultSkyboxMaterial != null)
        {
            RenderSettings.skybox = defaultSkyboxMaterial;
        }
    }
    
    
    public void loadSkyboxMaterial (Material skyBoxMaterial)
    {
        if (skyBoxMaterial != null)
        {
            RenderSettings.skybox = skyBoxMaterial;
        }
        
    }
}
