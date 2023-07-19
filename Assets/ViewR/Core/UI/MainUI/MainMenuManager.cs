using UnityEngine;
using ViewR.HelpersLib.SurgeExtensions.Animators.UI;

namespace ViewR.Core.UI.MainUI
{
    /// <summary>
    /// Main Menu Manager - makes it appear and disappear using the <see cref="UIFadeScale"/> class.
    /// </summary>
    [RequireComponent(typeof(UIFadeScale))]
    public class MainMenuManager : MonoBehaviour
    {
        private UIFadeScale _uiFadeScale;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _uiFadeScale = GetComponent<UIFadeScale>();
        }

        public void Close()
        {
            if(!_uiFadeScale)
                Initialize();
            
            _uiFadeScale.Appear(false);
        }

        public void Open()
        {
            if(!_uiFadeScale)
                Initialize();
            
            _uiFadeScale.Appear(true);
        }



    }
}
