using System;
using UnityEngine;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.Voice
{
    /// <summary>
    /// Adjusts the transparency of the given sprite according to the animation curve and syncs that value.
    /// 
    /// Should not need syncing, as each avatar has a RealtimeAvatarVoice component, we can do it all locally!
    /// </summary>
    [RequireComponent(typeof(Normal.Realtime.RealtimeAvatarVoice))]
    public class VoiceVolumeIndicator : MonoBehaviour
    {
        #region Serialized Fields and Properties
        
        [SerializeField] private AnimationCurve colorFadeCurve;
        [SerializeField] private UnityEngine.UI.Image volumeSprite;
        
        #endregion

        #region Public Members

        private float _volume;
        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                AdjustIndicatorTransparency(); // Should not need syncing, as each avatar has a RealtimeAvatarVoice component, we can do it all locally!
            }
        }
        
        #endregion

        #region Private Members
        private Normal.Realtime.RealtimeAvatarVoice realtimeAvatarVoice;
        private bool initialized;
        #endregion

        #region Unity specific methods
        
        private void Update()
        {
            // Abort if we're not online.
            if (!NetworkManager.IsInstanceRegistered ||
                !NetworkManager.Instance.MainRealtimeInstance.connected)
                return;
            
            if (!initialized)
                Initialize();

            // If still no realtimeAvatarVoice: try init again. Else throw error.
            if (realtimeAvatarVoice is null)
                if (!Initialize())
                    throw new MissingComponentException($"Couldn't find {nameof(Normal.Realtime.RealtimeAvatarVoice)} on {gameObject.name}.");

            // Bail if muted
            if (realtimeAvatarVoice.mute)
            {
                // Ensure visibility - but only once.
                if (Math.Abs(Volume - 1) > 0.03f)
                    Volume = 1;
                return;
            }

            // Do the math in the animation curve
            Volume = Mathf.Clamp01(colorFadeCurve.Evaluate(realtimeAvatarVoice.voiceVolume));
        }
        
        #endregion

        #region Private methods
        
        private bool Initialize()
        {
            // Get reference and return success
            initialized = TryGetComponent(out realtimeAvatarVoice);
            
            return initialized;
        }

        private void AdjustIndicatorTransparency()
        {
            // Apply the value.
            var tmpColor = volumeSprite.color;
            tmpColor.a = Volume;
            volumeSprite.color = tmpColor;
        }

        #endregion

    }
}