using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;

namespace ViewR.HelpersLib.Utils.UI
{
    /// <summary>
    /// A basic class to simulate button presses.
    /// Gets destroyed if not Unity editor!
    /// </summary>
    public class EmulateButtonPress : MonoBehaviour
    {
        [Header("Optional")]
        [SerializeField, Tooltip("Optional, gets auto populated.")]
        private Button button;

#if UNITY_EDITOR
        private void Awake()
        {
            if (!button)
                button = GetComponent<Button>();
        }

        [ExposeMethodInEditor]
        private void SimpleButtonPress()
        {
            var data = new PointerEventData(EventSystem.current)
            {
                button = PointerEventData.InputButton.Left
            };

            button.OnPointerClick(data);
        }

        private void OnValidate()
        {
            if (button) return;
        
            if (TryGetComponent(out Button localButton))
                button = localButton;
        }

#else
    private void Awake()
    {
        Destroy(this);
    }

#endif
    }
}