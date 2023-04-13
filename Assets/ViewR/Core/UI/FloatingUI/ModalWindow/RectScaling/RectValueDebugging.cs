using UnityEngine;

namespace ViewR.Core.UI.FloatingUI.ModalWindow.RectScaling
{
    /// <summary>
    /// Shows the RectTransform settings on screen.
    ///  ! Gets destroyed if not in editor !
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class RectValueDebugging : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Start()
        {
#if !UNITY_EDITOR
            Destroy(this);
#endif
            //Fetch the RectTransform from the GameObject
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnGUI()
        {
            if(_rectTransform.gameObject.activeInHierarchy)
                // Show the current Rect settings on the screen
                GUI.Label(new Rect(20, 20, 150, 80), "Rect : " + _rectTransform.rect);
        }
    }
}
