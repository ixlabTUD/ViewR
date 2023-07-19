using Normal.Realtime;
using TMPro;
using UnityEngine;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.Utils.ShowOnlineStatus
{
    public class ShowRoomName : ShowOnlineStatus
    {
        [Header("Setup")]
        [SerializeField]
        private string offlineText = "- - - - - - -";

        [Header("References")]
        [SerializeField]
        private TMP_Text tmpText;


        internal override void ShowConnectedToRoom(Realtime realtime)
        {
            base.ShowConnectedToRoom(realtime);
            
            tmpText.text = NetworkManager.Instance.GetRoomNameToJoin();
            tmpText.color = onlineColor;
        }

        internal override void ShowDisconnectedFromRoom(Realtime realtime)
        {
            base.ShowDisconnectedFromRoom(realtime);
            
            tmpText.text = offlineText;
            tmpText.color = offlineColor;
        }
    }
}