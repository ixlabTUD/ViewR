using System;
using System.Collections.Generic;
using System.Linq;
using Normal.Realtime;
using Pixelplacement;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.UserBinder
{
    public class IXUserManager : Singleton<IXUserManager>
    {
        private RealtimeAvatarManager normcoreManager;
        
        public List<RealtimeAvatar> avatars;

        public Vector4[] headPositions;
        
        //Interleaved hand positions
        // 0 LeftHand 1 RightHand
        public Vector4[] handPositions;

        public Texture2D headPositionsTexture2D;

        public Texture2D handPositionsTexture2D;
        
        public Vector4[] TextureTest;

        private void Start()
        {
            normcoreManager = NetworkManager.Instance.RealtimeAvatarManager;
            
            headPositionsTexture2D = new Texture2D(1,1, TextureFormat.RGBAFloat, false);
            
            handPositionsTexture2D = new Texture2D(2,1, TextureFormat.RGBAFloat, false);
        }

        private void Update()
        {
            if (!NetworkManager.Instance.MainRealtimeInstance.connected)
            {
                return;
            }

            avatars = normcoreManager.avatars.Values.ToList();
            if (avatars != null)
            {
                //SET HEAD POSITIONS
                headPositions = new Vector4[avatars.Count];
                for (int i = 0; i < avatars.Count; i++)
                {
                    headPositions[i] = avatars[i].GetComponent<IXAvatar>().head.position;
                }
                
                //SET HAND POSITIONS
                handPositions = new Vector4[avatars.Count * 2];
                for (int i = 0; i < avatars.Count; i+=2)
                {
                    handPositions[i] = avatars[i/2].GetComponent<IXAvatar>().leftHand.position;
                    handPositions[i+1] = avatars[i/2].GetComponent<IXAvatar>().rightHand.position;
                }
            }

            //CONVERT HEAD POSITIONS TO TEXTURE
            if (headPositions != null)
            {
                //CHECK IF NUMBER CHANGED
                if (headPositions.Length != headPositionsTexture2D.height)
                {
                    headPositionsTexture2D.Reinitialize(1, headPositions.Length);
                }
                
                NativeArray<Vector4> copyHeadPositions = new NativeArray<Vector4>(headPositions.Length,Allocator.TempJob);
                copyHeadPositions.CopyFrom(headPositions);
                headPositionsTexture2D.SetPixelData(copyHeadPositions, 0, 0);
                headPositionsTexture2D.Apply();
                copyHeadPositions.Dispose();
            }
            
            //CONVERT HAND POSITIONS INTO TEXTURE
            if (handPositions != null)
            {
                //CHECK IF NUMBER CHANGED
                if (handPositions.Length / 2 != handPositionsTexture2D.height)
                {
                    handPositionsTexture2D.Reinitialize(2, handPositions.Length /2);
                }
                
                NativeArray<Vector4> copyHandPositions = new NativeArray<Vector4>(handPositions.Length,Allocator.TempJob);
                copyHandPositions.CopyFrom(handPositions);
                handPositionsTexture2D.SetPixelData(copyHandPositions, 0, 0);
                handPositionsTexture2D.Apply();
                copyHandPositions.Dispose();
            }

            
            //TEXTURE TEST
            // if (avatarHeadPositionsTexture2D != null)
            // {
            //     TextureTest = new Vector4[avatarHeadPositionsTexture2D.height];
            //     for (int j = 0; j < avatarHeadPositionsTexture2D.height; j++)
            //     {
            //         TextureTest[j] = avatarHeadPositionsTexture2D.GetPixel(0, j);
            //     }
            // }
            
            
            // if (handPositionsTexture2D != null)
            // {
            //     TextureTest = new Vector4[handPositionsTexture2D.height * 2];
            //     for (int j = 0; j < handPositionsTexture2D.height; j+=2)
            //     {
            //         TextureTest[j] = handPositionsTexture2D.GetPixel(0, j/2);
            //         TextureTest[j+1] = handPositionsTexture2D.GetPixel(1, j/2);
            //     }
            // }
            
            
            
        }
    }
}
