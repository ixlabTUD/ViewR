using UnityEngine;
using UnityEngine.EventSystems;

namespace ViewR.HelpersLib.SurgeExtensions.Animators.UI
{
    /// <summary>
    /// Makes the UI appear 
    /// </summary>
    public class UIAppearCaller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private UIFloatAndFadeIn uiFloatAndFadeIn;
        
        #region Unity Methods, Handlers and Callbacks

        public void OnPointerEnter(PointerEventData eventData)
        {
            uiFloatAndFadeIn.Appear(appear: true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            uiFloatAndFadeIn.Appear(appear: false, true);
        }

        #endregion
        
    }
}
