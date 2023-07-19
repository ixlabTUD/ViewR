using UnityEngine;

namespace ViewR.StatusManagement.Setters
{
    /// <summary>
    /// Sets the values of the <see cref="ClientPhysicalLocationState"/>
    /// </summary>
    public class ClientPhysicalLocationSetter : MonoBehaviour
    {
        public static void SetClientLocation(ClientPhysicalLocation newPhysicalLocation)
        {
            ClientPhysicalLocationState.SetStatus(newPhysicalLocation);
        }

        public void SetClientLocationRemote() => SetClientLocation(ClientPhysicalLocation.Remote);
        public void SetClientLocationOnSite() => SetClientLocation(ClientPhysicalLocation.OnSite);
    }
}
