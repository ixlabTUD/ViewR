using UnityEngine;
using UnityEngine.EventSystems;
using ViewR.HelpersLib.Utils.ToggleObjects;

namespace ViewR.Core.UI.Visuals.AnimatedImageFill
{
    /// <summary>
    /// A simple class that calls the objects to toggle ON/OFF whenever pointer entered/exited.
    /// </summary>
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        internal ObjectsToToggle objectsToToggle;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            objectsToToggle.ToggleOn();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            objectsToToggle.ToggleOff();
        }
    }
}
