using UnityEngine;
using ViewR.HelpersLib.Utils.ToggleObjects;

namespace ViewR.HelpersLib.Utils
{
    /// <summary>
    /// "Syncs" the active state of <see cref="ObjectsToToggle"/> with this components enabled state!
    /// If this component gets disabled(<see cref="OnDisable"/>), every <see cref="ObjectsToToggle"/> gets disabled (unless <see cref="invert"/>, then they do get activated).
    /// Same goes for OnEnable.
    /// </summary>
    public class SetComponentActiveToThisComponentEnabledState : MonoBehaviour
    {
        [SerializeField]
        private ObjectsToToggle objectsToToggle;
        [SerializeField]
        private bool invert;

        private void OnEnable()
        {
            objectsToToggle.Enable((!invert) ? true : false);
        }

        private void OnDisable()
        {
            objectsToToggle.Enable((!invert) ? false : true);
        }
    }
}
