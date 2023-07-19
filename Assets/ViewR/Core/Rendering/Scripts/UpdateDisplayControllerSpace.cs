using System;
using UnityEngine;

namespace ViewR.Core.Rendering
{
    public class UpdateDisplayControllerSpace : MonoBehaviour
    {
        private void OnEnable()
        {
            //DisplayController.Instance.MakeObjectsTransparent(this.transform);
            DisplayController.Instance.spaceParent = transform;
            DisplayController.Instance.MakeObjectsTransparent(transform);
        }
    }
}
