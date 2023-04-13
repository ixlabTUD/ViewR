using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.SurgeExtensions.Animators.Parents;

namespace ViewR.HelpersLib.SurgeExtensions.Animators
{
    /// <summary>
    /// A class that allows to enable objects before <see cref="AppearIn"/>,
    /// and to disable objects, once <see cref="AppearOut"/> is completed.
    /// </summary>
    public class AppearerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Appearer[] appearers;

        [Header("Enable Callback")]
        [SerializeField]
        private GameObject[] gameObjectsToEnable;
        [SerializeField]
        private MonoBehaviour[] behavioursToEnable;
        [SerializeField]
        private Renderer[] renderersToEnable;

        [Header("Disable Callback")]
        [SerializeField]
        private GameObject[] gameObjectsToDisable;
        [SerializeField]
        private MonoBehaviour[] behavioursToDisable;
        [SerializeField]
        private Renderer[] renderersToDisable;
        

        #region Appearer Methods

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void AppearIn()
        {
            EnableObjects();
            foreach (var appearer in appearers)
                appearer.Appear(true);
        }

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void AppearOut()
        {
            foreach (var appearer in appearers)
                appearer.Appear(false, callback: DisableObjects);
        }
    
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void AppearInInverse()
        {
            EnableObjects();
            foreach (var appearer in appearers)
                appearer.Appear(true, true);
        }
        
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void AppearOutInverse()
        {
            foreach (var appearer in appearers)
                appearer.Appear(false, true, callback: DisableObjects);
        }

        #endregion

        #region Private Helpers

        private void EnableObjects()
        {
            foreach (var objectToEnable in gameObjectsToEnable)
                objectToEnable.SetActive(true);
            foreach (var behaviour in behavioursToEnable)
                behaviour.enabled = true;
            foreach (var rendererToEnable in renderersToEnable)
                rendererToEnable.enabled = true;
        }

        private void DisableObjects()
        {
            foreach (var objectToEnable in gameObjectsToDisable)
                objectToEnable.SetActive(false);
            foreach (var behaviour in behavioursToDisable)
                behaviour.enabled = false;
            foreach (var rendererToEnable in renderersToDisable)
                rendererToEnable.enabled = false;
        }

        #endregion
    
        #region Convenience features

        [ContextMenu("ForceAutoPopulate")]
        private void ForceAutoPopulate()
        {
            AutoPopulate(true);
        }
        private void AutoPopulate(bool overwriteAnyway = false)
        {
            if (appearers.Length == 0 || overwriteAnyway)
            {
                appearers = GetComponents<Appearer>();
            }
        }
        
        private void OnValidate()
        {
            AutoPopulate();
        }

        #endregion
    }
}
