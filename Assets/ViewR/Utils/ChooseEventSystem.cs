using UnityEngine;

namespace ViewR.Utils
{
    public class ChooseEventSystem : MonoBehaviour
    {
        public GameObject canvaspointable;
        public GameObject desktopEventSystem;
        public GameObject OVRRig;

        void Start()
        {
            if (OVRManager.isHmdPresent)
            {
                canvaspointable.SetActive(true);
                desktopEventSystem.SetActive(false);
            }
            else
            {
                desktopEventSystem.SetActive(true);
                canvaspointable.SetActive(false);
                OVRRig.transform.Translate(0, 1, 0);
            }
        }
    }
}
