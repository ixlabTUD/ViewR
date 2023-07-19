using Eflatun.SceneReference;
using UnityEngine.Events;
using ViewR.Core.UI.FloatingUI.Loading;
using ViewR.HelpersLib.Utils.ToggleObjects;

namespace ViewR.Core.Experiences.ExperienceSync
{
    [System.Serializable]
    public class ExperienceConfig
    {
        public string experienceTitle;
        public string experienceDisplayed;
        public SceneReference sceneToLoadAdditively;
        public ObjectsToToggle objectsToToggle;
        public UnityEvent toggledOn;
        public UnityEvent toggledOff;

        public void ToggleOn()
        {
            objectsToToggle.ToggleOn();
            toggledOn?.Invoke();

            // Load requested scene if configured
            AdditiveSceneLoadingManager.Instance.LoadConfiguredScene(sceneToLoadAdditively);
        }

        public void ToggleOff()
        {
            objectsToToggle.ToggleOff();
            toggledOff?.Invoke();

            // Don't unload a scene here - default state will load it.
        }
    }
}