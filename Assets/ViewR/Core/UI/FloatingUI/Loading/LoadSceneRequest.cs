using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ViewR.Core.UI.FloatingUI.Loading
{
    public class LoadSceneRequest : MonoBehaviour
    {
        [SerializeField]
        private SceneReference sceneToUse;

        /// <summary>
        /// Load the given scene
        /// </summary>
        public void LoadScene()
        {
            // Load async if not loaded
            if (sceneToUse != null &&
                !SceneManager.GetSceneByBuildIndex(sceneToUse.BuildIndex).isLoaded)
            {
                SceneManager.LoadSceneAsync(sceneToUse.BuildIndex, LoadSceneMode.Additive);
            }
        }

        /// <summary>
        /// Unloads the given scene.
        /// </summary>
        public void UnloadScene()
        {
            // First: Unload async if loaded
            if (sceneToUse != null &&
                SceneManager.GetSceneByBuildIndex(sceneToUse.BuildIndex).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneToUse.BuildIndex);
            }
        }
    }
}