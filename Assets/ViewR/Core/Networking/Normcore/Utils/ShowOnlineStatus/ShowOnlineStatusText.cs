using Normal.Realtime;
using TMPro;
using UnityEngine;

namespace ViewR.Core.Networking.Normcore.Utils.ShowOnlineStatus
{
    public class ShowOnlineStatusText : ShowOnlineStatus
    {
        [Header("Setup")]
        [SerializeField]
        private string onlineText;

        [SerializeField]
        private string offlineText;

        [Header("References")]
        [SerializeField]
        private TMP_Text tmpText;


        internal override void ShowConnectedToRoom(Realtime realtime)
        {
            base.ShowConnectedToRoom(realtime);
            
            tmpText.text = onlineText;
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