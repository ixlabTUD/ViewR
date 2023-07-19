using System;
using UnityEngine;
using ViewR.HelpersLib.Extensions.General;
using ViewR.Managers;

namespace ViewR.Core.OVR.Passthrough.Visuals.Blink
{
    /// <summary>
    /// Blink-like effect on the selected Passthrough layer via the <see cref="PassthroughManager"/>
    /// Can also be used via the static <see cref="BlinkLayer"/> method.
    ///
    /// <see cref="PassthroughLayerInOutManager"/> contains the single point of access for the logic to ensure we never run more than one effect at a time!
    /// </summary>
    public class BlinkPassthroughLayer : MonoBehaviour
    {
        public LayerType layerToManipulate;

        public void BlinkConfiguredLayer() => BlinkLayer(layerToManipulate);

        /// <summary>
        /// Blink-like effect on the given <see cref="LayerType"/>
        /// </summary>
        public static void BlinkLayer(LayerType layerToManipulate)
        {
            // Catch
            if (!PassthroughManager.IsInstanceRegistered)
            {
                Debug.LogError($"No {nameof(PassthroughManager)} registered yet. Bailing.".StartWithFrom(nameof(BlinkPassthroughLayer)));
                return;
            }

            // Hide / show !
            switch (layerToManipulate)
            {
                case LayerType.Main:
                    PassthroughManager.Instance.BlinkMainPassthrough();
                    break;
                case LayerType.Reprojected:
                    PassthroughManager.Instance.BlinkReprojectedPassthrough();
                    break;
                case LayerType.ReprojectedHighlighted:
                    PassthroughManager.Instance.BlinkReprojectedHighlightedPassthrough();
                    break;
                case LayerType.Overlay:
                    PassthroughManager.Instance.BlinkOverlayLayer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layerToManipulate), layerToManipulate, null);
            }
        }

        /// <summary>
        /// Blink-like effect on all layers.
        /// </summary>
        public static void BlinkAllLayers()
        {
            foreach (LayerType i in Enum.GetValues(typeof(LayerType)))
                BlinkLayer(i);
        }
    }
}