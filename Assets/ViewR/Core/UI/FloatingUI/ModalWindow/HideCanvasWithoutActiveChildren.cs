using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.Core.UI.FloatingUI.ModalWindow
{
    public class HideCanvasWithoutActiveChildren : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;
        [Help("If further objects should be considered")]
        [SerializeField]
        private GameObject[] additionalRelevantGameObjects;
        
        private MonoBehaviour[] _components;
        private List<GameObject> _childs = new List<GameObject>();
        private bool _currentlyActive;

        private void Awake()
        {
            DoCache();
        }

        private void DoCache()
        {
            _components = canvas.GetComponents<MonoBehaviour>();

            foreach (Transform child in canvas.transform)
                _childs.Add(child.gameObject);
        }

        private void Update()
        {
            var active = _childs.Any(child => child.activeSelf);
            foreach (var additionalRelevantGameObject in additionalRelevantGameObjects)
                active = additionalRelevantGameObject.activeSelf || active;

            if (active != _currentlyActive)
            {
                foreach (var component in _components)
                    component.enabled = active;

                _currentlyActive = active;
            }
            
        }
    }
}