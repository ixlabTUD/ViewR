using UnityEngine.SceneManagement;

namespace ViewR.HelpersLib.Extensions.SceneManagerExtensions
{
    public static class SceneManagerExtension
    {
        /// <summary>
        /// Gets all currently open / loaded scenes and returns an array.
        /// </summary>
        /// <returns></returns>
        public static Scene[] GetLoadedScenes()
        {
            var sceneCount = SceneManager.sceneCount;
            var loadedScenes = new Scene[sceneCount];

            for (var i = 0; i < sceneCount; i++)
                loadedScenes[i] = SceneManager.GetSceneAt(i);

            return loadedScenes;
        }
    }
}