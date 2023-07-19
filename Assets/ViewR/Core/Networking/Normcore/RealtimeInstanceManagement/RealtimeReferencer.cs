using Normal.Realtime;
using UnityEngine;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.RealtimeInstanceManagement
{
    /// <summary>
    /// A generic access point to reference or access any realtime instance configured! 
    /// </summary>
    /// <remarks>
    /// A class inheriting from this should re-implement to ensure it is accessible to the [ShowIf] attribute:
    ///      // Need to override this to make [ShowIf] in RealtimeReferencer work.
    ///     protected override bool ShowCustomField() => realtimeTypeToUse == RealtimeInstanceType.Custom;
    ///
    /// ToDo: Should no longer be a parent class. Turn into static class instead.
    /// </remarks>
    public class RealtimeReferencer : MonoBehaviour
    {
        public static Realtime RealtimeToUse 
        {
            get
            {
                if (NetworkManager.IsInstanceRegistered)
                    return NetworkManager.Instance.MainRealtimeInstance;
                return null;
            }
        }
    }
}
