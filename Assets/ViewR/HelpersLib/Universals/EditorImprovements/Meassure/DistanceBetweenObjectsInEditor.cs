using UnityEditor;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;

namespace ViewR.HelpersLib.Universals.EditorImprovements.Meassure
{
    [ExecuteInEditMode]
    public class DistanceBetweenObjectsInEditor : MonoBehaviour
    {
        [Help("This is only executed in EditorMode!", MessageType.Warning)]
        public Transform a;
        public Transform b;

        [SerializeField, ReadOnly]
        private Vector3 distanceXYZ;
        [SerializeField, ReadOnly]
        private float distance;
    
#if UNITY_EDITOR
        private void Update()
        {
            if (!a || !b)
                return;

            var bPosition = b.position;
            var aPosition = a.position;

            distanceXYZ = aPosition - bPosition;
            distance = Vector3.Distance(aPosition, bPosition);
        }
#endif
    }
}
