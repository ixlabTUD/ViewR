using System;
using TMPro;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;

namespace ViewR.Core.OVR
{
    public class TestingOVRPlugin : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text tmpBatteryLevel;
        [SerializeField]
        private TMP_Text tmpBatteryStatus;
        [SerializeField]
        private TMP_Text tmpShouldQuit;
        [SerializeField]
        private TMP_Text tmpShouldRecenter;


#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void Test()
        {
            // Performance settings
            OVRPlugin.suggestedGpuPerfLevel = OVRPlugin.ProcessorPerformanceLevel.SustainedHigh;
            OVRPlugin.suggestedCpuPerfLevel = OVRPlugin.ProcessorPerformanceLevel.SustainedHigh;
            OVRPlugin.foveatedRenderingLevel = OVRPlugin.FoveatedRenderingLevel.Medium;
            OVRPlugin.useDynamicFoveatedRendering = true;

            // Boundaries!
            OVRPlugin.GetBoundaryGeometry(OVRPlugin.BoundaryType.PlayArea);

            // What for?
            OVRPlugin.GetMesh(OVRPlugin.MeshType.HandLeft, out var newMesh);

            // Haptics
            OVRPlugin.SetControllerHaptics((uint) OVRPlugin.GetActiveController(),
                new OVRPlugin.HapticsBuffer {Samples = new IntPtr(3), SamplesCount = 6});
            OVRPlugin.SetControllerVibration((uint) OVRPlugin.GetActiveController(), 1, 2);

            // Dev Mode
            OVRPlugin.SetDeveloperMode(OVRPlugin.Bool.True);

            // Insight Mesh?
            // OVRPlugin.CreateInsightTriangleMesh();
            // OVRPlugin.occlusionMesh;

            // Checking active state of 
            Debug.Log($"IsInsightPassthroughInitialized: {OVRPlugin.IsInsightPassthroughInitialized()}");
            Debug.Log($"IsMixedRealityInitialized: {OVRPlugin.IsMixedRealityInitialized()}");

            // Spatial Entities
            // OVRPlugin.SpatialEntityCreateSpatialAnchor();
            // OVRPlugin.SpatialEntityEnumerateSupportedComponents();
            // OVRPlugin.SpatialEntityEraseSpatialEntity();
            // OVRPlugin.SpatialEntityQuerySpatialEntity();
            // OVRPlugin.SpatialEntityGetComponentEnabled();
            // OVRPlugin.SpatialEntitySaveSpatialEntity();
            // OVRPlugin.SpatialEntitySetComponentEnabled();
            // OVRPlugin.SpatialEntityTerminateSpatialEntityQuery();
            // OVRPlugin.SpatialEntityUuid uuid;


            tmpBatteryStatus.text = SystemInfo.batteryStatus.ToString();
            tmpBatteryLevel.text = SystemInfo.batteryLevel.ToString("P");

            tmpShouldRecenter.text = OVRPlugin.shouldRecenter.ToString();
            tmpShouldQuit.text = OVRPlugin.shouldQuit.ToString();
        }
    }
}