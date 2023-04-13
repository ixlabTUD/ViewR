#if ODIN_INSPECTOR
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace ViewR.Tools.BulkProcessing.Editor
{
    /// <summary>
    /// A link between a <see cref="Prefab"/> Asset and a <see cref="SceneGameObject"/> to allow create a PrefabVariant of the prefab based on the <see cref="SceneGameObject"/>.
    /// </summary>
    public class ConvertPair
    {
        public bool DoRun = true;

        [AssetsOnly]
        public GameObject Prefab;

        [SceneObjectsOnly]
        public GameObject SceneGameObject;

        public string TargetPathFolder;

        public string NamePostfix = "";

        public string TargetPath
        {
            get
            {
                if (!(TargetPathFolder.FastEndsWith("/") || TargetPathFolder.FastEndsWith("\\")))
                    TargetPathFolder += "\\";

                return TargetPathFolder + Prefab.name + "_" + SceneGameObject.name + NamePostfix + "_variant" + ".prefab";
            }
        }

        public void CreatePrefabVariant(bool forcePrefabRootScaleToOne)
        {
            if (!DoRun)
                return;

            // Catch
            if (File.Exists(TargetPath))
            {
                Debug.LogError("File already exists. Bailing.");
                return;
            }

            // Get prefabs and create instances
            var originalPrefab = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(Prefab)) as GameObject;
            var prefabInstance = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;

            if (prefabInstance == null)
                throw new MissingComponentException("prefabInstance is empty.");

            // Get first renderer
            var originalRenderer = prefabInstance.GetComponentInChildren<Renderer>();
            var originalMeshFilter = originalRenderer.GetComponent<MeshFilter>();
            var originalMeshCollider = originalRenderer.GetComponent<MeshCollider>();

            // Catches
            if (!originalRenderer)
            {
                Debug.LogError("Could not find a Renderer in the given prefabInstance. Bailing.");
                return;
            }

            if (!originalMeshFilter || !originalMeshCollider)
            {
                Debug.LogError(
                    "For now, we require a renderer, mesh filter and mesh collider on the object. Looked at " +
                    originalRenderer.gameObject.name);
                CleanUp(prefabInstance);
                return;
            }

            // Should be all good to continue!

            // Modify prefab
            // Set Pose
            prefabInstance.transform.SetPositionAndRotation(SceneGameObject.transform.position,
                SceneGameObject.transform.rotation);

            // Get Components from new object
            var newRenderer = SceneGameObject.GetComponentInChildren<Renderer>();
            var newCollider = newRenderer.GetComponent<MeshCollider>();
            var newMeshFilterMesh = newRenderer.GetComponent<MeshFilter>()?.sharedMesh;
            var newColliderMesh = newCollider ? newCollider.sharedMesh : newMeshFilterMesh;

            // Apply new values to prefab
            originalMeshCollider.sharedMesh = newColliderMesh;
            originalMeshFilter.sharedMesh = newMeshFilterMesh;

            // Update Materials
            var materials = new Material[newRenderer.sharedMaterials.Length];
            for (var i = 0; i < newRenderer.sharedMaterials.Length; i++)
                materials[i] = newRenderer.sharedMaterials[i];
            originalRenderer.sharedMaterials = materials;
            
            // Ensure everything gets updated.
            EditorUtility.SetDirty(originalMeshCollider);
            EditorUtility.SetDirty(originalMeshFilter);
            EditorUtility.SetDirty(originalRenderer);

            // Update Scale
            originalRenderer.transform.localScale = SceneGameObject.transform.localScale;
            if (forcePrefabRootScaleToOne)
                prefabInstance.transform.localScale = Vector3.one;

            // Save prefab variant and instantiate
            var prefabVariant =
                PrefabUtility.SaveAsPrefabAssetAndConnect(prefabInstance, TargetPath, InteractionMode.UserAction);
            var prefabVariantInstance = PrefabUtility.InstantiatePrefab(prefabVariant) as GameObject;

            // Ensure position in hierarchy
            prefabVariantInstance.transform.parent = SceneGameObject.transform.parent;

            Debug.Log("Written Asset Variant to TargetPath.");

            CleanUp(prefabInstance);
            // PrefabUtility.UnloadPrefabContents(prefabInstance);
            // PrefabUtility.UnloadPrefabContents(prefabVariant);
        }

        /// <summary>
        /// Update the path of the <see cref="TargetPathFolder"/>.
        /// </summary>
        public void UpdatePath()
        {
            if (!DoRun)
                return;

            if (Prefab == null)
            {
                TargetPathFolder = "Assets";
            }
            else if (AssetDatabase.GetAssetPath(Prefab).StartsWith("Packages"))
            {
                TargetPathFolder = "Assets";
            }
            else
            {
                TargetPathFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Prefab)) + "\\";
            }
        }

        /// <summary>
        /// Ensure we tidy up after ourselves.
        /// </summary>
        /// <param name="gameObjectToDelete"></param>
        private void CleanUp(GameObject gameObjectToDelete = null)
        {
            // Clean up
            if (gameObjectToDelete != null)
                GameObject.DestroyImmediate(gameObjectToDelete);
        }
    }
}
#endif