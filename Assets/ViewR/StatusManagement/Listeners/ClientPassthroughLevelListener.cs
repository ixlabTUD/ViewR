using System;
using UnityEngine;

namespace ViewR.StatusManagement.Listeners
{
    /// <summary>
    /// A basic listener.
    /// </summary>
    public class ClientPassthroughLevelListener : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            ClientPassthroughLevel.PassthroughLevelUpdated += HandlePassthroughLevelUpdate;
        }

        protected virtual void OnDisable()
        {
            ClientPassthroughLevel.PassthroughLevelUpdated -= HandlePassthroughLevelUpdate;
        }

        protected virtual void HandlePassthroughLevelUpdate(PassthroughLevel newPassthroughLevel)
        {
        }
    }
}