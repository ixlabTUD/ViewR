using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

namespace ViewR.Tools.BulkProcessing.Editor
{
#if ODIN_INSPECTOR
    /// <summary>
    /// An editor to make objects in the scene interactable!
    /// </summary>
    /// <remarks>See below in the DetailedInfoBox for further info.</remarks>
    public class MakeSingleObjectInteractableEditor : OdinEditorWindow
    {
        
        [MenuItem("Tools/Custom/Make Single Mesh Object Interactable")]
        private static void OpenWindow()
        {
            GetWindow<MakeSingleObjectInteractableEditor>().Show();

            var window = GetWindow<MakeSingleObjectInteractableEditor>();
            window.ConvertPair = new ConvertPair();
        }

        [DetailedInfoBox("Click to see the instructions!",
            "A quick Editor Tool that allows us to bulk-create PrefabVariants, i.e. for interactables.\n " +
            "Can be used with only a single convert pair, or with a list of convert pairs.\n " +
            "\nWhat it does:\n" +
            " - Takes the first MeshRenderer of the given scene object,\n" +
            " - Applies Mesh and Materials to the first MeshRenderer (and its collider etc.) of the given prefab.\n " +
            " - Creates PrefabVariant with new material.\n " +
            " - Positions (Pose+Scale) an instance of the PrefabVariant where the given scene object is positioned.\n " +

            "\nLimitations:\n" +
            " - Only works on objects that are represented by a single mesh.\n " +

            "\n Workflow:\n" +
            " - Optional: Configure the single ConvertPair to your likings. This will be applied to all ConvertPairs when running the next step.\n" +
            " - Select scene object(s) that you want to create Prefab-Variants of.\n " +
            " - Select the prefab that you want to create the variant of in the project window. (The window will show errors if not selecting an asset.)\n " +
            " - Validate the list and its settings. !This cannot always be undone!\n " +
            " - Click \"Run Bulk\".\n " +
            
            "\n Note:\n" +
            " - Configure ConvertPair and click \"Run Single Pair\" if you wish to only run on only one item.\n " +
            ""
            )]
        
        [FormerlySerializedAs("ConvertPairs")] 
        [TableList] public List<ConvertPair> convertPairs;

        public ConvertPair ConvertPair;
        
        public bool forcePrefabRootScaleToOne = true;

        [HorizontalGroup]
        [Button(ButtonSizes.Medium), GUIColor(0.2f, 1, 0)]
        public void RunSinglePair()
        {
            ConvertPair.CreatePrefabVariant(forcePrefabRootScaleToOne);
        }

        [HorizontalGroup]
        [Button(ButtonSizes.Medium), GUIColor(0.2f, 1, 0)]
        public void AutoFillPaths()
        {
            ConvertPair.UpdatePath();
            if (convertPairs != null && convertPairs.Count != 0)
                foreach (var convertPair in convertPairs)
                    convertPair.UpdatePath();
        }
        
        [Button(ButtonSizes.Medium), GUIColor(0.2f, 1, 0)]
        public void PopulateListWithSelectedObjects()
        {
            // Get selected objects
            var selections = Selection.transforms;
            
            // Check Names for duplicates
            var duplicatesPresent = CheckNamesForDuplicates(selections, out var duplicateKeys);
            if (duplicatesPresent)
                Debug.LogWarning("Found duplicates in the names of the selected objects. Their names should be changed to ensure uniqueness.");

            // Create a pair for each of them and populate the array
            var localConvertPairs = new List<ConvertPair>();
            for (var i = 0; i < selections.Length; i++)
            {
                var selection = selections[i];
                var localConvertPair = new ConvertPair
                {
                    DoRun = ConvertPair?.DoRun ?? true,
                    Prefab = ConvertPair?.Prefab,
                    SceneGameObject = selection.gameObject,
                    TargetPathFolder = ConvertPair?.TargetPathFolder ?? "Assets\\"
                };
                
                // Catch Naming
                if (duplicateKeys.Contains(selection.name))
                    localConvertPair.NamePostfix = i.ToString();
                
                localConvertPairs.Add(localConvertPair);
            }

            convertPairs = localConvertPairs;
            
            EditorUtility.SetDirty(this);
        }
        
        [Button(ButtonSizes.Medium), GUIColor(0.2f, 1, 0)]
        public void PopulateListPrefabsWithSelectedObject()
        {
            if (convertPairs == null || convertPairs.Count == 0)
            {
                Debug.LogError("\"convertPairs\" is null or empty. Cannot perform operation.");
                return;
            }
            
            // Get selected objects
            var selection = Selection.activeTransform;
            foreach (var convertPair in convertPairs)
            {
                convertPair.Prefab = selection.gameObject;
            }
            EditorUtility.SetDirty(this);
        }

        [Button(ButtonSizes.Medium), GUIColor(0.2f, 1, 0)]
        public void RunBulk()
        {
            foreach (var convertPair in convertPairs)
            {
                convertPair.CreatePrefabVariant(forcePrefabRootScaleToOne);
            }
        }


        /// <summary>
        /// Checks the given <see cref="selections"/> transforms for duplicate names.
        /// </summary>
        /// <param name="selections">The transforms to check.</param>
        /// <param name="duplicateKeys">The detected duplicates.</param>
        /// <returns>True if found duplicates.</returns>
        private bool CheckNamesForDuplicates(IEnumerable<Transform> selections, out List<string> duplicateKeys)
        {
            var transforms = selections.ToList();
            var selectionNames = transforms.Select(selection => selection.name).ToList();

            // Find duplicates
            duplicateKeys = selectionNames.GroupBy(objName => objName)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key).ToList();

            return duplicateKeys.Count > 0;
        }
    }

#endif
}