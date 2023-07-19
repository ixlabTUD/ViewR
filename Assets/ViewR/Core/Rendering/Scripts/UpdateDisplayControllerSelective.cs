using System;
using UnityEngine;

namespace ViewR.Core.Rendering
{
    public class UpdateDisplayControllerSelective : MonoBehaviour
    {
        private void OnEnable()
        {
            //DisplayController.Instance.MakeObjectsTransparent(this.transform);
            DisplayController.Instance.selectiveParent = transform;
            DisplayController.Instance.MakeObjectsTransparent(transform);
        }
    }
}
