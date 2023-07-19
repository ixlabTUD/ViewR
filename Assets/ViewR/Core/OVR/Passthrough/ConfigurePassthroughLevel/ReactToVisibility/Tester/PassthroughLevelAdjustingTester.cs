using System;
using UnityEngine;
using ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility;
using ViewR.HelpersLib.Extensions.General;
using ViewR.StatusManagement;
using ViewR.StatusManagement.Listeners;

namespace ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.ReactToVisibility.Tester
{
    /// <summary>
    /// Simplified class of <see cref="PassthroughLevelAdjuster"/> to test and debug listeners, such as <see cref="MaterialChangerEnvironment"/>, who react to these setters.
    /// </summary>
    public class PassthroughLevelAdjustingTester : ClientPassthroughLevelListener
    {
        #region Private Members

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
            
            switch (newPassthroughLevel)
            {
                // Video everywhere
                case PassthroughLevel.EntirelyPassthrough:
                    // Set visibility values.
                    ObjectVisibility.Visible = false;
                    UserAvatarVideoVisibility.Visible = true;
                    VirtualEnvironmentVisibility.Visible = false;
                    break;
                    
                // Video everywhere except digital objects of relevance
                case PassthroughLevel.MostlyPassthrough:
                    // Set visibility values.
                    ObjectVisibility.Visible = true;
                    UserAvatarVideoVisibility.Visible = true;
                    VirtualEnvironmentVisibility.Visible = false;
                    break;
                    
                // Passthrough level based on slider
                case PassthroughLevel.MixedMode:
                    // Note: Could put gradual value here, i.e. based on distance.

                    MixedModePassthroughVisibility.Visible = true;
                    
                    break;
                    
                // Video nowhere except for where other users are
                case PassthroughLevel.MostlyVirtual:
                    // Set visibility values.
                    ObjectVisibility.Visible = true;
                    UserAvatarVideoVisibility.Visible = true;
                    VirtualEnvironmentVisibility.Visible = true;
                    MixedModePassthroughVisibility.Visible = false;
                    
                    break;
                    
                // Video nowhere at all ( = remote mode, uses avatars)
                case PassthroughLevel.EntirelyVirtual:
                    // Set visibility values.
                    ObjectVisibility.Visible = true;
                    UserAvatarVideoVisibility.Visible = false;
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