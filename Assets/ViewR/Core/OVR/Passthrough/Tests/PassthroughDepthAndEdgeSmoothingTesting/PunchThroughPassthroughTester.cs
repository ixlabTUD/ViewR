using System.Collections;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.EditorExtensions.ReadOnly;

namespace ViewR.Core.OVR.Passthrough.Tests.PassthroughDepthAndEdgeSmoothingTesting
{
    public class PunchThroughPassthroughTester : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private Transform _parent;
        [SerializeField, ReadOnly]
        private GameObject _passthroughQuad;
        [SerializeField, ReadOnly]
        private Renderer _nonPassthrough;
        [SerializeField, ReadOnly]
        private OVRKeyboard.TrackedKeyboardInfo _activeKeyboardInfo;
        
        
        [SerializeField]
        private bool showKeyLabelLayer;
        
        /// <summary>
        /// How large of a passthrough area to show surrounding the keyboard when using Key Label presentation.
        /// </summary>
        [Tooltip("How large of a passthrough area to show surrounding the keyboard when using Key Label presentation")]
        public float PassthroughBorderMultiplier = 0.2f;

        /// <summary>
        /// The shader used for rendering the keyboard model in opaque mode.
        /// </summary>
        [Tooltip("The shader used for rendering the keyboard model")]
        public Shader nonPassthroughShader; // Mobile/Diffuse -->  = UnityEditor.ObjectWrapperJSON:{"guid":"0000000000000000f000000000000000","localId":10703,"type":0,"instanceID":284}
        /// <summary>
        /// Internal only. The shader used to render the passthrough rectangle in opaque mode.
        /// </summary>
        public Shader passthroughShader; // PunchThroughPassthrough --> UnityEditor.ObjectWrapperJSON:{"guid":"8120257754b48344b8a1a0961bad0c46","localId":4800000,"type":3,"instanceID":694}

        /// <summary>
        /// Internal only. The passthrough layer used to render the passthrough rectangle in key label mode.
        /// </summary>
        public OVRPassthroughLayer ProjectedPassthroughKeyLabel;
        private OVROverlay projectedPassthroughOpaque_;
        [SerializeField] private MeshFilter projectedPassthroughMesh;


        private bool _initialized;

        [ExposeMethodInEditor]
        public void LoadMesh()
        {
            if (!_initialized)
                InitializeKeyboardInfo();
            
            
            _passthroughQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            
            _passthroughQuad.transform.localPosition = new Vector3(0.0f, -0.01f, 0.0f);

            _passthroughQuad.transform.parent = _parent;
            
            _passthroughQuad.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);

            var borderSize = _activeKeyboardInfo.Dimensions.x * PassthroughBorderMultiplier;
            
            _passthroughQuad.transform.localScale = new Vector3(
                _activeKeyboardInfo.Dimensions.x + borderSize,
                _activeKeyboardInfo.Dimensions.z + borderSize,
                _activeKeyboardInfo.Dimensions.y);

            var meshRenderer = _passthroughQuad.GetComponent<MeshRenderer>();
            meshRenderer.material.shader = passthroughShader;
        }

        [ExposeMethodInEditor]
        public void NonPassthrough()
        {
            _nonPassthrough.material.shader = nonPassthroughShader;
        }

        private void InitializeKeyboardInfo()
        {
            _activeKeyboardInfo = new OVRKeyboard.TrackedKeyboardInfo
            {
                Name = "None",
                Dimensions = new Vector3(0.5f, 0f, 0.43f),
                Identifier = uint.MaxValue
            };

            _initialized = true;
        }
        
        
        private IEnumerator InitializeHandPresenceData()
        {
            GameObject ovrCameraRig = GameObject.Find("OVRCameraRig");
            if (ovrCameraRig == null)
            {
                Debug.LogError("Scene does not contain an OVRCameraRig");
                yield break;
            }

            projectedPassthroughOpaque_ = ovrCameraRig.AddComponent<OVROverlay>();

            projectedPassthroughOpaque_.currentOverlayShape = OVROverlay.OverlayShape.KeyboardHandsPassthrough;

            projectedPassthroughOpaque_.hidden = true;
            projectedPassthroughOpaque_.gameObject.SetActive(true);

            ProjectedPassthroughKeyLabel.hidden = true;
            ProjectedPassthroughKeyLabel.gameObject.SetActive(true);
        }

        void RegisterPassthroughMeshToSDK()
        {
            if (ProjectedPassthroughKeyLabel.IsSurfaceGeometry(projectedPassthroughMesh.gameObject))
            {
                ProjectedPassthroughKeyLabel.RemoveSurfaceGeometry(projectedPassthroughMesh.gameObject);
            }

            ProjectedPassthroughKeyLabel.AddSurfaceGeometry(projectedPassthroughMesh.gameObject, true);
        }

        public void test()
        {
            if (showKeyLabelLayer)
            {
                _nonPassthrough.material.shader = nonPassthroughShader;
                _passthroughQuad.SetActive(false);
                projectedPassthroughOpaque_.hidden = false;
                ProjectedPassthroughKeyLabel.hidden = true;
            }
            else
            {
                _nonPassthrough.material.shader = nonPassthroughShader;
                _passthroughQuad.SetActive(true);
                projectedPassthroughOpaque_.hidden = true;
                ProjectedPassthroughKeyLabel.hidden = false; // Always shown
            }
        }
    }
}