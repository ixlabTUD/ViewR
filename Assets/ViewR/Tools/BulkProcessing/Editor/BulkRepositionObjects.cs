using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

namespace ViewR.Tools.BulkProcessing.Editor
{
#if ODIN_INSPECTOR
    /// <summary>
    /// A quick Editor Tool that allows us to bulk-align objects.
    /// </summary>
    /// <remarks>See below in the DetailedInfoBox for infos.</remarks>
    public class BulkRepositionObjects : OdinEditorWindow
    {
        [MenuItem("Tools/Custom/BulkRepositionObjects")]
        private static void OpenWindow()
        {
            GetWindow<BulkRepositionObjects>().Show();

            var window = GetWindow<BulkRepositionObjects>();
            window.objectPairs = new List<ObjectPair>();
        }

        [DetailedInfoBox("Click to see the instructions!",
            "A quick Editor Tool that allows us to bulk-align objects.\n " +
            "\n Workflow:\n" +
            " - Select a series of sources, and click the button for populating sources.\n " +
            " - Select a series of targets, and click the button for populating targets.\n " +
            " - If you select only one object as target, it instantiates it as many times as their are sources added to the list (-1).\n " +
            " - Then it applies the scale and pose of each target to its paired source.")]
        
        [TableList][SerializeField] private List<ObjectPair> objectPairs;
        
        [PropertyOrder(10)]
        [Button(ButtonSizes.Medium), GUIColor(0.2f, 1, 0)]
        public void RunBulk()
        {
            foreach (var objectPair in objectPairs)
                objectPair.Align();
        }
        
        
        [HorizontalGroup]
        [Button(ButtonSizes.Medium), GUIColor(0.2f, 1, 0)]
        public void PopulateListWithSelectedSources()
        {
            // Get selected objects
            var selections = Selection.transforms;

            var localPairs = new List<ObjectPair>();
            foreach (var selection in selections)
            {
                var localPair = new ObjectPair()
                {
                    DoRun = true,
                    TargetObject = null,
                    SourceObject = selection
                };
                localPairs.Add(localPair);
            }

            objectPairs = localPairs;
            
            EditorUtility.SetDirty(this);
        }
        
        [HorizontalGroup]
        [Button(ButtonSizes.Medium), GUIColor(0.2f, 1, 0)]
        public void PopulateListWithSelectedTargets()
        {
            // Get selected objects
            var selections = Selection.transforms.ToList();

            if (selections.Count != objectPairs.Count && selections.Count != 1)
                throw new Exception($"Selections.Count != objectParis.Count. Maybe run {nameof(PopulateListWithSelectedSources)}");

            if (selections.Count == 1)
            {
                // spawn a few instances.
                for (var i = 0; i < objectPairs.Count-1; i++)
                {
                    var newObject = PrefabUtility.InstantiatePrefab(selections[0]) as Transform;
                    selections.Add(newObject);
                }
            }

            for (var i = 0; i < objectPairs.Count; i++)
            {
                objectPairs[i].TargetObject = selections[i];
            }
            
            EditorUtility.SetDirty(this);
        }
        
        internal class ObjectPair
        {
            public bool DoRun;
            [SceneObjectsOnly]
            public Transform SourceObject;
            [SceneObjectsOnly]
            public Transform TargetObject;

            public void Align()
            {
                if (!DoRun)
                    return;

                TargetObject.SetParent(SourceObject.parent);
                TargetObject.SetPositionAndRotation(SourceObject.position, SourceObject.rotation);
                TargetObject.localScale = SourceObject.localScale;
            }
        }
    }
#endif
}