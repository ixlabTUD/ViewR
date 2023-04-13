using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewR.Core.Networking.Normcore.UserBinder;

public class IXShadergraphBinderUserPositions : MonoBehaviour
{
    // Start is called before the first frame update

    private IXUserManager userManager;
    private Material shadergraphMaterial;

    [Header("Head Positions")]
    [SerializeField] private bool bindHeadPositons;
    [SerializeField] private string ShaderKeywordHeadPositions = "_HeadPositionTexture";
    
    [Header("Hand Positions")]
    [SerializeField] private bool bindHandPositions;
    [SerializeField] private string ShaderKeywordHandPositions = "_HandPositionTexture";
    
    void Start()
    {
        userManager = IXUserManager.Instance;
        shadergraphMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (bindHeadPositons)
        {
            if (shadergraphMaterial.HasTexture(ShaderKeywordHeadPositions))
            {
                shadergraphMaterial.SetTexture(ShaderKeywordHeadPositions, userManager.headPositionsTexture2D);
            }
            Debug.LogError("Shader has no head texture with keyword: " + ShaderKeywordHeadPositions);
            
        }
        
        if (bindHandPositions)
        {
            if (shadergraphMaterial.HasTexture(ShaderKeywordHandPositions))
            {
                shadergraphMaterial.SetTexture(ShaderKeywordHandPositions, userManager.handPositionsTexture2D);
            }
            
            Debug.LogError("Shader has no hand texture with keyword: " + ShaderKeywordHeadPositions);
        }
        
    }
}
