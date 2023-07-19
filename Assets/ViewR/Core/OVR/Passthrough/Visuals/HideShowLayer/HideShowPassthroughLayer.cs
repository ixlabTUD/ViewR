using System;
using System.Collections;
using UnityEngine;
using ViewR.HelpersLib.Extensions.General;
using ViewR.Managers;

namespace ViewR.Core.OVR.Passthrough.Visuals.HideShowLayer
{
    /// <summary>
    /// Hides/Shows the selected Passthrough layer via the <see cref="PassthroughManager"/>
    /// Can also be used via the static <see cref="HideLayer"/> method.
    /// </summary>
    public class HideShowPassthroughLayer : MonoBehaviour
    {
        public LayerType layerToManipulate;

        public void HideConfiguredLayer(bool hideLayer) => HideLayer(layerToManipulate, hideLayer);
        public void HideConfiguredLayer() => HideLayer(layerToManipulate, true);
        public void ShowConfiguredLayer() => HideLayer(layerToManipulate, false);

        public static void HideLayer(LayerType layerToManipulate, bool hideLayer)
        {
            // Catch
            if (!PassthroughManager.IsInstanceRegistered)
            {
                Debug.LogError($"No {nameof(PassthroughManager)} registered yet. Bailing.".StartWithFrom(nameof(HideShowPassthroughLayer)));
                return;
            }

            // Hide / show !
            switch (layerToManipulate)
            {
                case LayerType.Main:
                    PassthroughManager.Instance.HideMainPassthrough(hideLayer);
                    break;
                case LayerType.Reprojected:
                    PassthroughManager.Instance.HideReprojectedPassthrough(hideLayer);
                    break;
                case LayerType.ReprojectedHighlighted:
                    PassthroughManager.Instance.HideReprojectedHighlightedPassthrough(hideLayer);
                    break;
                case LayerType.Overlay:
                    PassthroughManager.Instance.HideOverlayPassthrough(hideLayer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layerToManipulate), layerToManipulate, null);
            }
        }

        public static void HideAllLayers()
        {
            foreach (LayerType i in Enum.GetValues(typeof(LayerType)))
                HideLayer(i, true);
        }

        public IEnumerator HideLayerDelayed(LayerType layerToHide, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            HideLayer(layerToHide, true);
        }
    }
}