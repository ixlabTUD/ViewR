using System;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;
using ViewR.Managers;
using ViewR.StatusManagement;
using ViewR.StatusManagement.States;

namespace ViewR.Core.Networking.Normcore
{
    public class PassthroughSettingsSync : RealtimeComponent<PassthroughSettingsModel>
    {
        [ReadOnly]
        public bool useLocalSettings = false;
        // [SerializeField]
        // private Slider geometryOpacitySlider;
        [SerializeField]
        private DisplayController displayController;

        [Header("User Representation")]
        [SerializeField]
        private Toggle avatarToggle;
        [SerializeField]
        private Toggle videoToggle;
        [SerializeField]
        private Toggle ikToggle;

        private const float SliderUpdateThreshold = 0.01f;
        
        [FormerlySerializedAs("geometryOpacityDidChange")] public UnityEvent<float> passthroughSpaceOpacityDidChange;
        public UnityEvent<float> passthroughSelectiveOpacityDidChange;

        
        protected override void OnRealtimeModelReplaced(PassthroughSettingsModel previousModel,
            PassthroughSettingsModel currentModel)
        {
            if (previousModel != null)
            {
                // Unregister from events
                previousModel.passthroughLevelDidChange -= UpdateLevel;
                previousModel.passthroughOverlayDidChange -= UpdateOverlay;
                previousModel.edgeFilterDidChange -= UpdateEdgeFilter;
                previousModel.avatarModeDidChange -= UpdateAvatarMode;
                previousModel.passthroughOpacitySpaceDidChange -= UpdateOpacitySpace;
                previousModel.passthroughOpacitySelectiveDidChange -= UpdateOpacitySelective;
            }

            if (currentModel != null)
            {
                // If this is a model that has no data set on it, populate it with the current mesh renderer color.
                if (currentModel.isFreshModel)
                {
                    currentModel.passthroughOpacitySpace = displayController.defaultOpacity;
                    currentModel.passthroughOpacitySelective = displayController.defaultOpacity;
                    currentModel.passthroughLevel = (int) ClientPassthroughLevel.CurrentPassthroughLevel;
                    currentModel.passthroughOverlay =
                        PassthroughManager.Instance.passthroughOverlayManager.GetOverlayOpacity();
                    currentModel.edgeFilter =
                        PassthroughManager.Instance.passthroughOverlayEdgeOpacityStyler.GetOverlayEdgeOpacity();
                    currentModel.avatarMode = UserRepresentation.CurrentUserRepresentationType;
                }

                // Apply current values
                UpdateOpacitySpace(currentModel, currentModel.passthroughOpacitySpace);
                UpdateOpacitySelective(currentModel, currentModel.passthroughOpacitySelective);
                UpdateLevel(currentModel, currentModel.passthroughLevel);
                UpdateOverlay(currentModel, currentModel.passthroughOverlay);
                UpdateEdgeFilter(currentModel, currentModel.edgeFilter);
                UpdateAvatarMode(currentModel, currentModel.avatarMode);

                currentModel.passthroughOpacitySpaceDidChange += UpdateOpacitySpace;
                currentModel.passthroughOpacitySelectiveDidChange += UpdateOpacitySelective;
                currentModel.passthroughLevelDidChange += UpdateLevel;
                currentModel.passthroughOverlayDidChange += UpdateOverlay;
                currentModel.edgeFilterDidChange += UpdateEdgeFilter;
                currentModel.avatarModeDidChange += UpdateAvatarMode;
            }
        }

        private void UpdateLevel(PassthroughSettingsModel passthroughSettingsModel, int value)
        {
            // Catch if using local settings BUT we got a realtime model when called
            if (useLocalSettings && passthroughSettingsModel != null)
                return;
            
            ClientPassthroughLevel.SetStatus((PassthroughLevel) value);
        }

        private void UpdateOverlay(PassthroughSettingsModel passthroughSettingsModel, float value)
        {
            // Catch if using local settings BUT we got a realtime model when called
            if (useLocalSettings && passthroughSettingsModel != null)
                return;
            
            PassthroughManager.Instance.passthroughOverlayManager.SetOverlayOpacity(value);
        }

        private void UpdateEdgeFilter(PassthroughSettingsModel passthroughSettingsModel, float value)
        {
            // Catch if using local settings BUT we got a realtime model when called
            if (useLocalSettings && passthroughSettingsModel != null)
                return;
            
            PassthroughManager.Instance.passthroughOverlayEdgeOpacityStyler.SetEdgeOpacity(value);
        }

        private void UpdateAvatarMode(PassthroughSettingsModel passthroughSettingsModel, UserRepresentationType value)
        {
            // Catch if using local settings BUT we got a realtime model when called
            if (useLocalSettings && passthroughSettingsModel != null)
                return;
            
            UserRepresentation.SetNewAvatarVisual(value);
        }

        private void UpdateOpacitySpace(PassthroughSettingsModel passthroughSettingsModel, float value)
        {
            // Catch if using local settings BUT we got a realtime model when called
            if (useLocalSettings && passthroughSettingsModel != null)
                return;
            
            displayController.updateOpacity(value);
            if (Math.Abs(value - displayController.defaultOpacity) > SliderUpdateThreshold)
                displayController.defaultOpacity = value;
            passthroughSpaceOpacityDidChange?.Invoke(value);
        }
        
        private void UpdateOpacitySelective(PassthroughSettingsModel passthroughSettingsModel, float value)
        {
            // Catch if using local settings BUT we got a realtime model when called
            if (useLocalSettings && passthroughSettingsModel != null)
                return;
            
            displayController.updateSelectiveOpacity(value);
            if (Math.Abs(value - displayController.defaultOpacity) > SliderUpdateThreshold)
                displayController.defaultOpacity = value;
            passthroughSelectiveOpacityDidChange?.Invoke(value);
        }



        public void SetLevel(float passthroughLevel)
        {
            // Pass it through
            SetLevel((int) passthroughLevel);
        }
        
        public void SetLevel(PassthroughLevel passthroughLevel)
        {
            // Pass it through
            SetLevel((int) passthroughLevel);
        }
        
        public void SetLevel(int value)
        {
            if (useLocalSettings)
            {
                UpdateLevel(null, value);
                return;
            }

            model.passthroughLevel = value;
        }

        public void SetOverlay(float value)
        {
            if (useLocalSettings)
            {
                UpdateOverlay(null, value);
                return;
            }

            model.passthroughOverlay = value;
        }

        public void SetEdgeFilter(float value)
        {
            if (useLocalSettings)
            {
                UpdateEdgeFilter(null, value);
                return;
            }

            model.edgeFilter = value;
        }

        public void SetOpacitySpace(float value)
        {
            if (useLocalSettings)
            {
                UpdateOpacitySpace(null, value);
                return;
            }

            model.passthroughOpacitySpace = value;
        }
        
        public void SetOpacitySelective(float value)
        {
            if (useLocalSettings)
            {
                UpdateOpacitySelective(null, value);
                return;
            }

            model.passthroughOpacitySelective = value;
        }

        #region Avatar Setters

        public void SetAvatarMode(int value)
        {
            SetAvatarMode((UserRepresentationType) value);
        }

        public void SetAvatarMode(UserRepresentationType value)
        {
            if (useLocalSettings)
            {
                // Only apply locally
                UpdateAvatarMode(null, value);
                return;
            }

            model.avatarMode = value;
        }

        public void SetUserRepresentationAvatar(bool doApply)
        {
            if (!doApply)
                return;
            SetAvatarMode(UserRepresentationType.HeadOnly);
        }
        public void SetUserRepresentationPrimitive(bool doApply)
        {
            if (!doApply)
                return;
            SetAvatarMode(UserRepresentationType.GeometricPrimitive);
        }
        public void SetUserRepresentationIK(bool doApply)
        {
            if (!doApply)
                return;
            SetAvatarMode(UserRepresentationType.IK);
        }

        #endregion

        /// <summary>
        /// Toggles the <see cref="useLocalSettings"/> variable.
        /// If we came from local and now don't use the local settings anymore, apply the models values.
        /// </summary>
        public void ToggleButton()
        {
            useLocalSettings = !useLocalSettings;

            if (!useLocalSettings)
            {
                UpdateLevel(model, model.passthroughLevel);
                UpdateOverlay(model, model.passthroughOverlay);
                UpdateEdgeFilter(model, model.edgeFilter);
                UpdateAvatarMode(model, model.avatarMode);
            }
        }

        public float GetCurrentOpacitySpace()
        {
            return model.passthroughOpacitySpace;
        }
        
        public float GetCurrentOpacitySelective()
        {
            return model.passthroughOpacitySelective;
        }
        
    }
}