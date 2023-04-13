using System;
using UnityEditor;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.Utils.ObjectDuplication
{
    /// <summary>
    /// Required for <see cref="DuplicationSender"/> and <see cref="DuplicationReceiver"/> to work.
    /// Essentially only holds references needed for their procedures.
    /// </summary>
    public class DuplicatableObject : MonoBehaviour
    {
        [SerializeField, Help("This needs to be the prefab from the project directory! Not the GameObject in the scene! Normcore only takes strings to instantiate objects.", MessageType.Warning)]
        private string prefabName;
        
        [SerializeField]
        private Transform parentWithRealtimeTransform;

        public string GetPrefabName() => prefabName;
        public Transform GetParentWithRealtimeTransform() => parentWithRealtimeTransform;
    }
}