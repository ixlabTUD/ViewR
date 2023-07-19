using Pixelplacement;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.EditorExtensions.ShowAttribute;

namespace ViewR.Core.OVR.Performance
{
    /// <summary>
    /// A class to access and set the Quests performance settings. 
    /// </summary>
    public class DevicePerformanceManager : SingletonExtended<DevicePerformanceManager>
    {
        [Header("GPU")]
        [SerializeField]
        private bool setGpuLevel = true;
        [SerializeField, ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.AND, nameof(setGpuLevel))]
        private int gpuLevel = 4;
        
        [Header("CPU")]
        [SerializeField]
        private bool setCpuLevel = true;
        [SerializeField, ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.AND, nameof(setCpuLevel))]
        private int cpuLevel = 4;
        
        [Header("Fixed Foveated Rendering")]
        [SerializeField]
        private bool setFixedFoveatedRenderingLevel = true;
        [SerializeField, ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.AND, nameof(setFixedFoveatedRenderingLevel))]
        private OVRPlugin.FoveatedRenderingLevel fixedFoveatedRenderingLevel = OVRPlugin.FoveatedRenderingLevel.Medium;
        
        [Header("Dynamic Fixed Foveated Rendering")]
        [SerializeField]
        private bool setUseDynamicFixedFoveatedRendering = true;
        [SerializeField, ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.AND, nameof(setUseDynamicFixedFoveatedRendering))]
        private bool useDynamicFixedFoveatedRendering = true;
        
        [Header("Apply High values on start")]
        [SerializeField]
        private bool applyHighValuesOnStart = true;

        private static bool _debugging;

        // System default values for Quest 1:
        private static OVRPlugin.ProcessorPerformanceLevel _defaultGpuLevel = (OVRPlugin.ProcessorPerformanceLevel) 1;
        private static OVRPlugin.ProcessorPerformanceLevel _defaultCpuLevel = (OVRPlugin.ProcessorPerformanceLevel) 1;
        private static OVRPlugin.FoveatedRenderingLevel _defaultFixedFoveatedRenderingLevel = OVRPlugin.FoveatedRenderingLevel.Off;
        private static bool _defaultUseDynamicFixedFoveatedRenderingLevel = false;
        private static bool _initializedPerformance;

        public static void SetPerformance(
            OVRPlugin.ProcessorPerformanceLevel? gpuLevel = null,
            OVRPlugin.ProcessorPerformanceLevel? cpuLevel = null,
            OVRPlugin.FoveatedRenderingLevel? fixedFoveatedRenderingLevel = null,
            bool? useDynamicFixedFoveatedRendering = null)
        {
            InitializePerformance();
            
            if (gpuLevel != null)
                OVRPlugin.suggestedGpuPerfLevel = (OVRPlugin.ProcessorPerformanceLevel) gpuLevel;

            if (cpuLevel != null)
                OVRPlugin.suggestedCpuPerfLevel = (OVRPlugin.ProcessorPerformanceLevel) cpuLevel;

            if (fixedFoveatedRenderingLevel != null)
                OVRPlugin.foveatedRenderingLevel =
                    (OVRPlugin.FoveatedRenderingLevel) fixedFoveatedRenderingLevel;

            if (useDynamicFixedFoveatedRendering != null)
                OVRPlugin.useDynamicFoveatedRendering = (bool) useDynamicFixedFoveatedRendering;
        }

        private void Start()
        {
            InitializePerformance();

            if (applyHighValuesOnStart)
            {
                UseEditorConfiguration();
            }
        }

        private static void InitializePerformance(bool overwriteInit = false)
        {
            if ( !overwriteInit && _initializedPerformance) return; 
            
            if (_debugging)
                Debug.Log("Performance Report:\n" +
                          "gpuLevel: " + OVRPlugin.suggestedGpuPerfLevel + "\n" +
                          "cpuLevel: " + OVRPlugin.suggestedCpuPerfLevel + "\n" +
                          "fixedFoveatedRenderingLevel:" + OVRPlugin.foveatedRenderingLevel + "\n" +
                          "useDynamicFixedFoveatedRendering: " + OVRPlugin.useDynamicFoveatedRendering);

            // Fetch default values.
            _defaultGpuLevel = OVRPlugin.suggestedGpuPerfLevel;
            _defaultCpuLevel = OVRPlugin.suggestedCpuPerfLevel;
            _defaultFixedFoveatedRenderingLevel = OVRPlugin.foveatedRenderingLevel;
            _defaultUseDynamicFixedFoveatedRenderingLevel = OVRPlugin.useDynamicFoveatedRendering;

            
            _initializedPerformance = true;
        }

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void ApplyPerformance(
            OVRPlugin.ProcessorPerformanceLevel? gpuLevel = null,
            OVRPlugin.ProcessorPerformanceLevel? cpuLevel = null,
            OVRPlugin.FoveatedRenderingLevel? fixedFoveatedRenderingLevel = null,
            bool? useDynamicFixedFoveatedRendering = null)
        {
            SetPerformance(gpuLevel, cpuLevel, fixedFoveatedRenderingLevel, useDynamicFixedFoveatedRendering);
        }

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void UseEditorConfiguration()
        {
            SetPerformance(
                setGpuLevel ? (OVRPlugin.ProcessorPerformanceLevel?) gpuLevel : null,
                setCpuLevel ? (OVRPlugin.ProcessorPerformanceLevel?) cpuLevel : null,
                setFixedFoveatedRenderingLevel ? fixedFoveatedRenderingLevel : (OVRPlugin.FoveatedRenderingLevel?) null,
                setUseDynamicFixedFoveatedRendering ? useDynamicFixedFoveatedRendering : (bool?) null
                );
        }

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void RestoreDefaults()
        {
            SetPerformance(
                setGpuLevel ? (OVRPlugin.ProcessorPerformanceLevel?) _defaultGpuLevel : null,
                setCpuLevel ? (OVRPlugin.ProcessorPerformanceLevel?) _defaultCpuLevel : null,
                setFixedFoveatedRenderingLevel ? _defaultFixedFoveatedRenderingLevel : (OVRPlugin.FoveatedRenderingLevel?) null,
                setUseDynamicFixedFoveatedRendering ? _defaultUseDynamicFixedFoveatedRenderingLevel : (bool?) null
                );
        }
        
        public static void RestoreDefaultValues()
        {
            SetPerformance(
                (OVRPlugin.ProcessorPerformanceLevel?) DevicePerformanceManager._defaultGpuLevel,
                (OVRPlugin.ProcessorPerformanceLevel?) DevicePerformanceManager._defaultCpuLevel,
                DevicePerformanceManager._defaultFixedFoveatedRenderingLevel,
                DevicePerformanceManager._defaultUseDynamicFixedFoveatedRenderingLevel
                );
        }
        
        public static void RestoreDefaultGPU()
        {
            SetPerformance(
                (OVRPlugin.ProcessorPerformanceLevel?) DevicePerformanceManager._defaultGpuLevel,
                null,
                null,
                null
                );
        }
        public static void RestoreDefaultCPU()
        {
            SetPerformance(
                null,
                (OVRPlugin.ProcessorPerformanceLevel?) DevicePerformanceManager._defaultCpuLevel,
                null,
                null
                );
        }
        
        public static void RestoreDefaultFfrLevel()
        {
            SetPerformance(
                null,
                null,
                DevicePerformanceManager._defaultFixedFoveatedRenderingLevel,
                null
                );
        }
        public static void RestoreDefaultUseDynmaicFfr()
        {
            SetPerformance(
                null,
                null,
                null,
                DevicePerformanceManager._defaultUseDynamicFixedFoveatedRenderingLevel
                );
        }
        
    }
}