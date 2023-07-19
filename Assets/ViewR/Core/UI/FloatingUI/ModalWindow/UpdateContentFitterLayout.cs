using UnityEngine;
using UnityEngine.UI;

namespace ViewR.Core.UI.FloatingUI.ModalWindow
{
    /// <summary>
    /// Refreshes the layout from the bottom up
    /// (since the parent objects will need the size of its children. The following makes sure to rebuild the children first an then the parent.
    ///
    /// Inspired by
    /// https://forum.unity.com/threads/content-size-fitter-refresh-problem.498536/#post-6857996
    ///
    /// Can be easily accessed through the <see cref="ModalWindowUIController"/> singelton
    /// </summary>
    public class UpdateContentFitterLayout : MonoBehaviour
    {
        private void Awake()
        {
            RecalculateLayouts();
        }
     
        public void RecalculateLayouts()
        {
            var rectTransform = (RectTransform)transform;
            RecalculateLayouts(rectTransform);
        }
     
        private void RecalculateLayouts(RectTransform rectTransform)
        {
            if (rectTransform == null || !rectTransform.gameObject.activeSelf)
            {
                return;
            }
         
            foreach (Transform child in rectTransform)
            {
                // catch:
                if(child is RectTransform rectChild)
                    RecalculateLayouts(rectChild);
                else
                    Debug.LogWarning("This layout contains a transform that is not a rectTransform. This may be okay.", this);
            }
     
            var layoutGroup = rectTransform.GetComponent<LayoutGroup>();
            var contentSizeFitter = rectTransform.GetComponent<ContentSizeFitter>();
            if (layoutGroup != null)
            {
                layoutGroup.SetLayoutHorizontal();
                layoutGroup.SetLayoutVertical();
            }
     
            if (contentSizeFitter != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

}