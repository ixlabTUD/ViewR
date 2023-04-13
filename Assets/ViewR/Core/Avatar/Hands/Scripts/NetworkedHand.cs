using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;
using static OVRSkeleton;

namespace ViewR.Core.Avatar.Hands.Scripts
{
    public class NetworkedHand : MonoBehaviour
    {
        public GameObject targetHandMesh;

        private List<Transform> _boneList = new List<Transform>();

        private OVRSkeleton _ovrSkeleton;
        private IOVRSkeletonDataProvider _dataProvider;

        private static readonly string[] BoneNames =
        {
            "Hand_Start",
            "Hand_ForearmStub",
            "Hand_Thumb0",
            "Hand_Thumb1",
            "Hand_Thumb2",
            "Hand_Thumb3",
            "Hand_Index1",
            "Hand_Index2",
            "Hand_Index3",
            "Hand_Middle1",
            "Hand_Middle2",
            "Hand_Middle3",
            "Hand_Ring1",
            "Hand_Ring2",
            "Hand_Ring3",
            "Hand_Pinky0",
            "Hand_Pinky1",
            "Hand_Pinky2",
            "Hand_Pinky3",
            "Hand_ThumbTip",
            "Hand_IndexTip",
            "Hand_MiddleTip",
            "Hand_RingTip",
            "Hand_PinkyTip"
        };

        private void Awake()
        {
            // Get refs
            _ovrSkeleton = GetComponent<OVRSkeleton>();

            // Setup Bone mapping
            var boneRoot = targetHandMesh.transform.Find("Bones");
            foreach (var name in BoneNames)
                _boneList.Add(boneRoot.FindChildRecursive(name));
        }

        private void Start()
        {
            // Request ownership for children networked transforms
            var transforms = targetHandMesh.GetComponentsInChildren<RealtimeTransform>();
            foreach (var rt in transforms)
                rt.RequestOwnership();
        }

        private void Update()
        {
            // Get provider
            if (_dataProvider == null && _ovrSkeleton != null)
                _dataProvider = _ovrSkeleton.GetComponent<IOVRSkeletonDataProvider>();
            // Apply if we have one now
            else if (_dataProvider != null)
            {
                var data = _dataProvider.GetSkeletonPoseData();

                if (data.IsDataValid)
                {
                    targetHandMesh.transform.parent.localScale = transform.localScale;
                    for (var i = 0; i < _boneList.Count; ++i)
                    {
                        if (_boneList[i] == null)
                        {
                            continue;
                        }

                        var rot = data.BoneRotations[i].FromQuatf();
                        rot.x = -rot.x;
                        rot.w = -rot.w;
                        _boneList[i].transform.localRotation = rot;
                    }
                }
            }
        }
    }
}