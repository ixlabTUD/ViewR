using System;
using UnityEngine;
using ViewR.HelpersLib.Extensions.General;
using ViewR.Managers;

namespace ViewR.Core.OVR.Passthrough.Visuals.Fader
{
    /// <summary>
    /// Fades the selected Passthrough layer via the <see cref="PassthroughManager"/>
    /// Can also be used via the static <see cref="FadeLayer"/> method.
    ///
    /// <see cref="PassthroughLayerInOutManager"/> contains the single point of access for the logic to ensure we never run more than one effect at a time!
    /// </summary>
    public class FadePassthroughLayer : MonoBehaviour
    {
        public LayerType layerToFade;

        public void FadeConfiguredLayer(bool fadeIn) => FadeLayer(layerToFade, fadeIn);
        public void FadeConfiguredLayerIn() => FadeLayer(layerToFade, true);
        public void FadeConfiguredLayerOut() => FadeLayer(layerToFade, false);

        /// <summary>
        /// Fades a layer in or out, and, by default, hides layers once they are faded out.
        /// This frees up performance overhead and should be done whenever possible.
        /// It also activates Layers if they were inactive.
        /// </summary>
        /// <param name="layerToFade">The <see cref="LayerType"/> to fade</param>
        /// <param name="fadeIn">Are we fading IN or OUT?</param>
        /// <param name="hideLayerOnceCompleted">Only evaluated if fading out. Should we hide the layer afterwards?</param>
        /// <param name="bailIfAlreadyVisible">Should we not fade if the layer is not <see cref="OVRPassthroughLayer.hidden"/>?</param>
        public static void FadeLayer(LayerType layerToFade, bool fadeIn, bool hideLayerOnceCompleted = true, bool bailIfAlreadyVisible = false)
        {
            // Catch
            if (!PassthroughManager.IsInstanceRegistered)
            {
                Debug.LogError(message: $"No {nameof(PassthroughManager)} registered yet. Bailing.".StartWithFrom(type: nameof(FadePassthroughLayer)));
                return;
            }

            // Fade!
            switch (layerToFade)
            {
                case LayerType.Main:
                    PassthroughManager.Instance.FadeMainPassthrough(fadeIn: fadeIn,
                        hideAfterFadeOut: hideLayerOnceCompleted,
                        bailIfAlreadyVisible: bailIfAlreadyVisible);
                    break;
                case LayerType.Reprojected:
                    PassthroughManager.Instance.FadeReprojectedPassthrough(fadeIn: fadeIn,
                        hideAfterFadeOut: hideLayerOnceCompleted,
                        bailIfAlreadyVisible: bailIfAlreadyVisible);
                    break;
                case LayerType.ReprojectedHighlighted:
                    PassthroughManager.Instance.FadeReprojectedHighlightedPassthrough(fadeIn: fadeIn,
                        hideAfterFadeOut: hideLayerOnceCompleted,
                        bailIfAlreadyVisible: bailIfAlreadyVisible);
                    break;
                case LayerType.Overlay:
                    PassthroughManager.Instance.FadeOverlayLayer(fadeIn: fadeIn,
                        hideAfterFadeOut: hideLayerOnceCompleted,
                        bailIfAlreadyVisible: bailIfAlreadyVisible);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(paramName: nameof(layerToFade), actualValue: layerToFade, message: null);
            }
        }
    }
}