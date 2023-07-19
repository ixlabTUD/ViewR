 using UnityEngine;
 using System;
 using System.Collections.Generic;
 using Pixelplacement;
 using UnityEditor;
 using UnityEngine.Rendering;
 using UnityEngine.Serialization;
 using ViewR.Core.Networking.Normcore;
 using ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.ReactToVisibility;
 using ViewR.StatusManagement;

 public class DisplayController : SingletonExtended<DisplayController>
 {
     delegate void SpaceGotPassthrough(float opacity);
     private SpaceGotPassthrough spaceGotPassthrough;
     
     
     
     [Range(0,1f)]
     public float defaultOpacity = 0.5f;

     public PassthroughSettingsSync passthroughSettingsSync;
     [FormerlySerializedAs("geometryParent")] [FormerlySerializedAs("defaultParent")] public Transform spaceParent;
     public Transform selectiveParent;


     public ShaderVariantCollection ShaderVariantCollection;
     
     private PassthroughLevel? previousPassthroughLevel;


     private OpacityLevel selectiveOpacityLevel;
     
     
     private const float AlphaClipValue = 0.01f;

     public enum SurfaceType
     {
         Opaque,
         Transparent
     }
     public enum BlendMode
     {
         Alpha,
         Premultiply,
         Additive,
         Multiply
     }
     
     public enum OpacityLevel
     {
         Passthrough,
         Virtual
     }

 // public void Start()
 // {
 //
 //     MakeObjectsTransparent(spaceParent);
 // }

 public void updateOpacity(float opacitySlider)
 {
     switch (opacitySlider)
     {
         case <= AlphaClipValue when previousPassthroughLevel == null:
             previousPassthroughLevel = ClientPassthroughLevel.CurrentPassthroughLevel;
             passthroughSettingsSync.SetLevel(PassthroughLevel.MostlyPassthrough);
             break;
         case > AlphaClipValue when previousPassthroughLevel != null:
             passthroughSettingsSync.SetLevel((PassthroughLevel) previousPassthroughLevel);
             previousPassthroughLevel = null;
             break;
         case > AlphaClipValue when previousPassthroughLevel == null:
             passthroughSettingsSync.SetLevel(PassthroughLevel.MostlyVirtual);
             break;
     }

     if (opacitySlider <= AlphaClipValue)
     {
         return;
     }

     try
     {
         foreach (var renderer in spaceParent.GetComponentsInChildren<MeshRenderer>())
         {

             foreach (var mat in renderer.materials)
             {

                 if (mat.HasColor("_BaseColor"))
                 {
                     mat.SetColor("_BaseColor",
                         new Color(mat.color.r, mat.color.g, mat.color.b,
                             opacitySlider));
                 } else if (mat.HasFloat("_Alpha"))
                 {
                     mat.SetFloat("_Alpha", opacitySlider);
                 }

                 if (mat.HasFloat("_Blend"))
                 {
                     mat.SetFloat("_Blend", (float)3f);
                 }
             }
         }
     }
     catch (Exception e)
     {
        Console.WriteLine(e);
     }
 }
 
 public void updateSelectiveOpacity(float opacitySlider)
 {
     switch (opacitySlider)
     {
         case <= AlphaClipValue when previousPassthroughLevel == null:
             previousPassthroughLevel = ClientPassthroughLevel.CurrentPassthroughLevel;
             passthroughSettingsSync.SetLevel(PassthroughLevel.MostlyPassthrough);
             break;
         case > AlphaClipValue when previousPassthroughLevel != null:
             passthroughSettingsSync.SetLevel((PassthroughLevel) previousPassthroughLevel);
             previousPassthroughLevel = null;
             break;
         case > AlphaClipValue when previousPassthroughLevel == null:
             passthroughSettingsSync.SetLevel(PassthroughLevel.MostlyVirtual);
             break;
     }
     
     if (opacitySlider <= AlphaClipValue)
     {
         return;
     }

     try
     {
         foreach (var renderer in selectiveParent.GetComponentsInChildren<MeshRenderer>())
         {

             foreach (var mat in renderer.materials)
             {

                 if (mat.HasColor("_BaseColor"))
                 {
                     mat.SetColor("_BaseColor",
                         new Color(mat.color.r, mat.color.g, mat.color.b,
                             opacitySlider));
                 } else if (mat.HasFloat("_Alpha"))
                 {
                     mat.SetFloat("_Alpha", opacitySlider);
                 }

                 if (mat.HasFloat("_Blend"))
                 {
                     mat.SetFloat("_Blend", (float)3f);
                 }
             }
         }
     }
     catch (Exception e)
     {
         Console.WriteLine(e);
     }
 }



 public void MakeObjectsTransparent(Transform parentGameObject)
 {
     //spaceParent = parentGameObject;
     
     foreach (var renderer in parentGameObject.GetComponentsInChildren<MeshRenderer>())
     {
             foreach (var mat in renderer.materials)
             {
                 mat.SetFloat("_Surface", (float)SurfaceType.Transparent);
                 mat.SetFloat("_AlphaClip",1f);
                 mat.SetFloat("_Blend", (float)0f);
                 mat.SetFloat("_Cutoff", AlphaClipValue);
                 mat.renderQueue = 4000;


                 if (mat.HasColor("_BaseColor"))
                 {
                     
                     mat.SetColor("_BaseColor",
                         new Color(mat.color.r, mat.color.g, mat.color.b,
                             defaultOpacity));
                 }
                 
                 if (mat.HasFloat("_Metallic"))
                 {
                     //mat.SetFloat("_Metallic", mat2.GetFloat("_Metallic"+ 0.01f));
                     continue;
                 }
                 
                 var shader = mat.shader;
                 
                 // Create and cache the LocalKeyword
                 LocalKeyword alphaTestKeyword = new LocalKeyword(shader, "_ALPHATEST_ON");
                 //LocalKeyword alphaBlendKeyword = new LocalKeyword(shader, "_ALPHABLEND_ON");
                 LocalKeyword alphaPremultiplyKeyword = new LocalKeyword(shader, "_ALPHAPREMULTIPLY_ON");
                 LocalKeyword surfaceTypeTransparentKeyword = new LocalKeyword(shader, "_SURFACE_TYPE_TRANSPARENT");
                 mat.EnableKeyword(alphaTestKeyword);
                 //mat.EnableKeyword(alphaBlendKeyword);
                 mat.EnableKeyword(alphaPremultiplyKeyword);
                 mat.EnableKeyword(surfaceTypeTransparentKeyword);
                 

                 for (int i = 0; i < 14; i++)
                 {
                     try
                     {
                         ShaderVariantCollection.Add(new ShaderVariantCollection.ShaderVariant(mat.shader,(PassType) i, mat.shaderKeywords));
                 
                     }
                     catch (Exception e)
                     {
                         Console.WriteLine(e);
                     }
                 }

                 
             }
     }

     parentGameObject.GetComponent<AddPassthroughMaterialSwapperToChildren>().AddComponentToAllChildrenWithRenderers();

    }
 }