using UnityEngine;
using UnityEngine.UI;

namespace ViewR.Core.OVR.UX.Testing
{
    public class ColorChangeConfirmation : MonoBehaviour
    {
        public Image image;

        public void ChangeBGColor()
        {
            image.color = Random.ColorHSV();
        
        }
    }
}
