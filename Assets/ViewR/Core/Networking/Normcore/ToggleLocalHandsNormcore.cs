using Normal.Realtime;
using UnityEngine;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore
{
    /// <summary>
    /// Toggles (shows / hides) the local hands if we are going online/offline
    /// Uses Normcore.
    /// </summary>
    [System.Obsolete("Deprecated - No longer applicable with OVR V37 and onwards.")]
    public class ToggleLocalHandsNormcore : MonoBehaviour
    {
        [SerializeField]
        private Realtime realtime;

        private HandReferences _handRefL;
        private HandReferences _handRefR;
        private bool _subscribed;

        private void Update()
        {
            if(realtime == null)
                if (NetworkManager.Instance.MainRealtimeInstance == null)
                    return;
                else
                {
                    realtime = NetworkManager.Instance.MainRealtimeInstance;
                    Subscribe();
                }
            
        }

        private void OnEnable()
        {
            Subscribe();
        }
        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            if(_subscribed) return;
            
            realtime.didConnectToRoom += RealtimeOndidConnectToRoom;
            realtime.didDisconnectFromRoom += RealtimeOndidDisconnectFromRoom;
            _subscribed = true;
        }

        private void Unsubscribe()
        {
            if(!_subscribed) return;
            
            realtime.didConnectToRoom -= RealtimeOndidConnectToRoom;
            realtime.didDisconnectFromRoom -= RealtimeOndidDisconnectFromRoom;
            _subscribed = false;
        }


        private void RealtimeOndidConnectToRoom(Realtime realtime1)
        {
            if(_handRefL == null)
                _handRefL = new HandReferences(OvrReferenceManager.Instance.LeftOvrHand);
            if(_handRefR == null)
                _handRefR = new HandReferences(OvrReferenceManager.Instance.RightOvrHand);

            _handRefL.ToggleHands(false);
            _handRefR.ToggleHands(false);
        }
        
        
        private void RealtimeOndidDisconnectFromRoom(Realtime realtime1)
        {
            if(_handRefL == null)
                _handRefL = new HandReferences(OvrReferenceManager.Instance.LeftOvrHand);
            if(_handRefR == null)
                _handRefR = new HandReferences(OvrReferenceManager.Instance.RightOvrHand);

            _handRefL.ToggleHands(true);
            _handRefR.ToggleHands(true);
        }
    }

    internal class HandReferences
    {
        public OVRHand OvrHand;
        public OVRMesh OvrMesh;
        public OVRMeshRenderer OvrMeshRenderer;
        public SkinnedMeshRenderer SkinnedMeshRenderer;

        public HandReferences(OVRHand ovrHand)
        {
            OvrHand = ovrHand;
            OvrMesh = ovrHand.GetComponent<OVRMesh>();
            OvrMeshRenderer = ovrHand.GetComponent<OVRMeshRenderer>();
            SkinnedMeshRenderer = ovrHand.GetComponent<SkinnedMeshRenderer>();
        }

        public void ToggleHands(bool show)
        {
            OvrMesh.enabled = OvrMeshRenderer.enabled = SkinnedMeshRenderer.enabled = show;
        }
    }
}