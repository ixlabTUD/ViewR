using UnityEngine;

namespace ViewR.Core.OVR.Passthrough.SelectivePassthroughModified
{
    public class fsOverlayPassthrough : MonoBehaviour
    {
        private OVRPassthroughLayer _passthroughLayer;
        private bool _variableOpacity;

        private void Start()
        {
            var ovrCameraRig = GameObject.Find("OVRCameraRig");
            if (ovrCameraRig == null)
            {
                Debug.LogError("Scene does not contain an OVRCameraRig");
                return;
            }

            _passthroughLayer = ovrCameraRig.GetComponent<OVRPassthroughLayer>();
            if (_passthroughLayer == null)
            {
                Debug.LogError("OVRCameraRig does not contain an OVRPassthroughLayer component");
            }
        }

        private void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.Start))
                _passthroughLayer.hidden = !_passthroughLayer.hidden;

            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
            {
                _variableOpacity = !_variableOpacity;
            
                // Only set Opacity to one once and not in update loop.
                if(!_variableOpacity)
                    _passthroughLayer.textureOpacity = 1;
            }

            // Modify Opacity whenever joystick is moved. 
            if(_variableOpacity)
            {
                var thumbstickX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
                _passthroughLayer.textureOpacity = thumbstickX * 0.5f + 0.5f;
            }
        }
    }
}