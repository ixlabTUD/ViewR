using Pixelplacement;
using UnityEngine;
using ViewR.HelpersLib.Utils.ToggleObjects;

namespace ViewR.Core.OVR.ControllerCenterPoint
{
    /// <summary>
    /// A manager class to toggle the given visuals from anywhere
    /// </summary>
    public class ControllerCenterPointManager : SingletonExtended<ControllerCenterPointManager>
    {
        [Header("Setup")]
        [SerializeField]
        private bool showOnStart;

        
        [Header("References")]
        [SerializeField]
        private ObjectsToToggle centerPointVisuals;


        private void Start()
        {
            ShowHints(showOnStart);
        }

        public void DoShow() => ShowHints(true);

        public void DoHide() => ShowHints(false);

        /// <summary>
        /// Shows or Hides all given <see cref="centerPointVisuals"/>
        /// </summary>
        /// <param name="show"></param>
        public void ShowHints(bool show)
        {
            // Show/Hide everything
            centerPointVisuals.Enable(show);
        }
    }
}
