using System;
using UnityEngine;
using ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility;
using ViewR.Core.OVR.Passthrough.Overlay;
using ViewR.Core.OVR.Passthrough.Visuals;
using ViewR.Core.OVR.Passthrough.Visuals.Blink;
using ViewR.Core.OVR.Passthrough.Visuals.Fader;
using ViewR.HelpersLib.Extensions.General;
using ViewR.Managers;
using ViewR.StatusManagement;
using ViewR.StatusManagement.Listeners;

namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel
{
    /// <summary>
    /// Reacts to changes in the <see cref="PassthroughLevel"/> of <see cref="ClientPassthroughLevel"/>.
    /// Provides an interface to configure the systems visual behaviour.
    /// 
    /// Uses classes like the <see cref="VirtualEnvironmentVisibility"/> and <see cref="UserAvatarVideoVisibility"/> to communicate efficiently to all objects.
    ///
    /// We don't really need to access this class because it will be automatically called by overriding <see cref="ClientPassthroughLevelListener.HandlePassthroughLevelUpdate"/>.
    /// But it has to live somewhere persistently :)
    /// </summary>
    public class PassthroughLevelAdjuster : ClientPassthroughLevelListener
    {
        [SerializeField]
        private PassthroughManager passthroughManager;
        
        #region Private Members

        private bool _mainLayerWasActive;
        private bool _reconstructedLayerWasActive;
        private bool _reconstructedHighlightLayerWasActive;
        private bool _overlayLayerWasActive;

        private OVRPassthroughLayer _cachedMainPassthroughLayer;
        private OVRPassthroughLayer _cachedUserDefinedPassthroughLayer;
        private OVRPassthroughLayer _cachedUserDefinedPassthroughHighlightLayer;
        private OVRPassthroughLayer _cachedOverlayLayer;

        private bool _initialized;

        private PassthroughLevel _previousPassthroughLevel;
        private float _previousOpacityValue;

        #endregion

        #region Static Events

        public delegate void PassthroughLevelChanged(PassthroughLevel previousPassthroughLevel, PassthroughLevel newPassthroughLevel);
        public static event PassthroughLevelChanged PassthroughLevelDidChange;

        #endregion

        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();

            Initialize();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // Reset.
            _initialized = false;
        }

        #endregion

        #region Init

        private void Initialize()
        {
            _cachedMainPassthroughLayer = passthroughManager.mainPassthroughLayer;
            _cachedUserDefinedPassthroughLayer = passthroughManager.userDefinedPassthroughLayer;
            _cachedUserDefinedPassthroughHighlightLayer = passthroughManager.userDefinedPassthroughLayerHighlighter;
            _cachedOverlayLayer = passthroughManager.overlayPassthroughLayer;

            _initialized = true;
        }

        #endregion

        #region Main Logic, reacting to changes in ClientPassthroughLevel.PassthroughLevelUpdated

        protected override void HandlePassthroughLevelUpdate(PassthroughLevel newPassthroughLevel)
        {
            if (!_initialized)
                Initialize();
            
            base.HandlePassthroughLevelUpdate(newPassthroughLevel);

            // Catch!
            if (!_initialized)
            {
                Debug.LogError("Could not initialize! Bailing!", this);
                return;
            }
            
            if(_previousPassthroughLevel == newPassthroughLevel)
            {
                Debug.LogWarning("Received the same level! Bailing".StartWithFrom(GetType()), this);
                return;
            }
                

            // Restore previous state if we previous == EntirelyVirtual
            if(_previousPassthroughLevel == PassthroughLevel.EntirelyVirtual)
            {
                // If the layer was active and is currently not active:
                if (_mainLayerWasActive && _cachedMainPassthroughLayer.hidden)
                    FadePassthroughLayer.FadeLayer(LayerType.Main, true);
                if (_reconstructedLayerWasActive && _cachedUserDefinedPassthroughLayer.hidden)
                    FadePassthroughLayer.FadeLayer(LayerType.Reprojected, true);
                if (_reconstructedHighlightLayerWasActive && _cachedUserDefinedPassthroughHighlightLayer.hidden)
                    FadePassthroughLayer.FadeLayer(LayerType.ReprojectedHighlighted, true);
            }
            else if (_previousPassthroughLevel == PassthroughLevel.EntirelyPassthrough)
            {
                // Ensure we hide the overlay if it wasn't active before we made it. This saves a lot of performance!
                passthroughManager.passthroughOverlayManager.SetOverlayOpacity(_previousOpacityValue);
                if (!_overlayLayerWasActive && !_cachedOverlayLayer.hidden)
                {
                    passthroughManager.FadeOverlayLayer(false);
                }
            }
            
            // Cache current state
            _overlayLayerWasActive = _cachedOverlayLayer.hidden;

            switch (newPassthroughLevel)
            {
                // Video everywhere
                case PassthroughLevel.EntirelyPassthrough:
                    // Save previous value
                    if (_previousPassthroughLevel != PassthroughLevel.EntirelyPassthrough)
                        _previousOpacityValue = _cachedOverlayLayer.textureOpacity;
                    
                    // Just enable a Passthrough layer that is set to "overlay"
                    passthroughManager.FadeOverlayLayer(true);
                    
                    // Set Opacity to defined max value.
                    passthroughManager.passthroughOverlayManager.SetOverlayOpacity(PassthroughOverlayManager.MAX_OVERLAY_OPACITY);

                    // Set visibility values.
                    ObjectVisibility.Visible = false;
                    UserAvatarVideoVisibility.Visible = true;
                    VirtualEnvironmentVisibility.Visible = false;
                    break;
                    
                // Video everywhere except digital objects of relevance
                case PassthroughLevel.MostlyPassthrough:
                    // Ensure the main layer is set to Underlay
                    PassthroughManager.UpdateMainPassthroughLayerOverlayType(OVROverlay.OverlayType.Underlay);
                    
                    // // Blink!
                    // BlinkPassthroughLayer.BlinkLayer(LayerType.Main);

                    // Set visibility values.
                    ObjectVisibility.Visible = true;
                    UserAvatarVideoVisibility.Visible = true;
                    VirtualEnvironmentVisibility.Visible = false;
                    break;

                // Passthrough level based on slider
                // Make objects subscribe to MixedModePassthroughVisibility to have them react to this stage!
                case PassthroughLevel.MixedMode:
                    // Ensure the main layer is set to Underlay
                    PassthroughManager.UpdateMainPassthroughLayerOverlayType(OVROverlay.OverlayType.Underlay);

                    // // Blink!
                    // BlinkPassthroughLayer.BlinkLayer(LayerType.Main);
                    
                    // Show PT materials on listening objects.
                    MixedModePassthroughVisibility.Visible = true;

                    // Note: Does not do anything yet. Could put gradual value here, i.e. based on distance.

                    break;

                // Video nowhere except for where other users are
                case PassthroughLevel.MostlyVirtual:
                    // Ensure the main layer is set to Underlay
                    PassthroughManager.UpdateMainPassthroughLayerOverlayType(OVROverlay.OverlayType.Underlay);

                    // If coming from EntirelyVirtual: Fade in, else: blink.
                    if (_previousPassthroughLevel == PassthroughLevel.EntirelyVirtual)
                        FadePassthroughLayer.FadeLayer(LayerType.Main, true);
                    // else
                    //     BlinkPassthroughLayer.BlinkLayer(LayerType.Main);

                    // Set visibility values.
                    ObjectVisibility.Visible = true;
                    UserAvatarVideoVisibility.Visible = true;
                    VirtualEnvironmentVisibility.Visible = true;
                    
                    // Stop showing PT materials on listening objects.
                    MixedModePassthroughVisibility.Visible = false;

                    break;
                    
                // Video nowhere at all ( = remote mode, uses avatars)
                case PassthroughLevel.EntirelyVirtual:
                    // Cache previous states.
                    _mainLayerWasActive = _cachedMainPassthroughLayer.hidden;
                    _reconstructedLayerWasActive = _cachedUserDefinedPassthroughLayer.hidden;
                    _reconstructedHighlightLayerWasActive = _cachedUserDefinedPassthroughHighlightLayer.hidden;
                    
                    // Fade them out!
                    FadePassthroughLayer.FadeLayer(LayerType.Main, false);
                    FadePassthroughLayer.FadeLayer(LayerType.Reprojected, false);
                    FadePassthroughLayer.FadeLayer(LayerType.ReprojectedHighlighted, false);

                    // Set visibility values.
                    ObjectVisibility.Visible = true;
                    UserAvatarVideoVisibility.Visible = true;
                    VirtualEnvironmentVisibility.Visible = true;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(newPassthroughLevel), newPassthroughLevel, null);
            }

            FirePassthroughLevelDidChange(_previousPassthroughLevel, newPassthroughLevel);

            _previousPassthroughLevel = newPassthroughLevel;
        }

        #endregion

        #region Helpers

        #endregion

        #region Event Invokers

        private static void FirePassthroughLevelDidChange(PassthroughLevel previousPassthroughLevel, PassthroughLevel newPassthroughLevel)
        {
            PassthroughLevelDidChange?.Invoke(previousPassthroughLevel, newPassthroughLevel);
        }

        #endregion
    }
}