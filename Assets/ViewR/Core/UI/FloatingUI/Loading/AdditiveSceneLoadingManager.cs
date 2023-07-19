using Eflatun.SceneReference;
using Pixelplacement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Utils.ToggleObjects;

namespace ViewR.Core.UI.FloatingUI.Loading
{
    /// <summary>
    /// This class toggles between a couple of given scenes.
    /// Single-Choice by design!
    /// </summary>
    public class AdditiveSceneLoadingManager : SingletonExtended<AdditiveSceneLoadingManager>
    {
        [Header("Default State")]
        [SerializeField]
        private ObjectsToToggle objectsOfDefaultState;

        private bool _inDefaultState = true;
        private SceneReference _additionallyLoadedScene;

        #region Public Methods

        /// <summary>
        /// Loads the given scene
        /// </summary>
        public void LoadConfiguredScene(SceneReference newScene)
        {
            // Catch no scene is passed, i.e. loading default state.
            if (string.IsNullOrEmpty(newScene.AssetGuidHex) || newScene.AssetGuidHex == "00000000000000000000000000000000" || newScene.AssetGuidHex.Contains("00000000000000000000000000000000"))
                return;

            // Catch if not in build
            if (newScene.BuildIndex < 0)
            {
                Debug.LogError("Scene not in build index!");
                return;
            }

            // Disable default state if needed
            DisableDefaultState();

            // Do the loading.
            LoadAndUnloadSceneAsync(newScene);
        }

        /// <summary>
        /// Loads the default state
        /// Also unloads all toggleable scenes!
        /// </summary>
#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void LoadDefaultState(bool forceEnable = false)
        {
            // Bail
            if (_inDefaultState && !forceEnable)
                return;

            // Enable default state
            objectsOfDefaultState.ToggleOn();
            _inDefaultState = true;

            // Unload every toggleable scene
            LoadAndUnloadSceneAsync();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Force disables the Default state.
        /// </summary>
        private void DisableDefaultState(bool forceDisable = false)
        {
            // Bail
            if (!_inDefaultState && !forceDisable)
                return;

            // Disable it!
            AdditiveSceneLoadingManager.Instance.objectsOfDefaultState.ToggleOff();
            _inDefaultState = false;
        }

        private void LoadAndUnloadSceneAsync(SceneReference sceneToLoad = null)
        {
            // First: Unload async if loaded
            if (_additionallyLoadedScene != null &&
                SceneManager.GetSceneByBuildIndex(_additionallyLoadedScene.BuildIndex).isLoaded)
                SceneManager.UnloadSceneAsync(_additionallyLoadedScene.BuildIndex);

            // Load async if not loaded
            if (sceneToLoad != null &&
                !SceneManager.GetSceneByBuildIndex(sceneToLoad.BuildIndex).isLoaded)
                SceneManager.LoadSceneAsync(sceneToLoad.BuildIndex, LoadSceneMode.Additive);

            // Cache Scene
            _additionallyLoadedScene = sceneToLoad;
        }

        #endregion
    }
}