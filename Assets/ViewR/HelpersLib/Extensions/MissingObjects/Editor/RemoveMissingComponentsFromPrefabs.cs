#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ViewR.HelpersLib.Extensions.MissingObjects.Editor
{
    /// <summary>
    /// Heavily inspired by https://forum.unity.com/threads/remove-all-missing-reference-behaviours.286808/#post-7206238, which in turn was heavily inspired from https://www.programmersought.com/article/431675062/
    /// thanks!
    /// </summary>
    public static class RemoveMissingComponentsFromPrefabs
    {
        /// <summary>
        /// DOES :
        /// Remove missing scripts in prefabs found at PATH, then save prefab.
        /// Saved prefab will have no missing scripts left.
        /// Will not mod prefabs that dont have missing scripts.
        ///
        /// NOTE :
        /// If prefab has another prefab#2 that is not in PATH, that prefab#2 will still have missing scripts.
        /// The instance of the prefab#2 in prefab will not have missing scripts (thus counted has override of prefab#2)
        ///
        /// HOW TO USE :
        /// Copy code in script anywhere in project.
        /// Set the PATH var in method <see cref="RemoveMissingScripstsInPrefabsAtPath"/>.
        /// Clik the button.
        /// </summary>
   
        [MenuItem("Tools/Editor/Remove MissingComponents in Prefabs at Path")]
        public static void RemoveMissingScripstsInPrefabsAtPath()
        {
            const string PATH = "Assets/Prefabs";


            EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", 0);
            var ids = AssetDatabase.FindAssets("t:Prefab", new string[] { PATH});
            for (var i = 0; i < ids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(ids[i]);
                var prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
   
                var delCount = 0;
                RecursivelyModifyPrefabChilds(instance, ref delCount);
   
                if (delCount > 0)
                {
                    Debug.Log($"Removed({delCount}) on {path}", prefab);
                    PrefabUtility.SaveAsPrefabAssetAndConnect(instance, path, InteractionMode.AutomatedAction);
                }
   
                UnityEngine.Object.DestroyImmediate(instance);
                EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", i / (float)ids.Length);
            }
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }
   
        private static void RecursivelyModifyPrefabChilds(GameObject obj, ref int delCount)
        {
            if (obj.transform.childCount > 0)
            {
                for (var i = 0; i < obj.transform.childCount; i++)
                {
                    var childObj = obj.transform.GetChild(i).gameObject;
                    RecursivelyModifyPrefabChilds(childObj, ref delCount);
                }
            }
   
            var innerDelCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
            delCount += innerDelCount;
        }
   
    }
}
   
    #endif