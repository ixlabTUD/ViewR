using UnityEngine;
using ViewR.Core.Networking.Normcore;
using ViewR.StatusManagement.States;

namespace ViewR.StatusManagement.Setters
{
    public class UserRepresentationSetter : MonoBehaviour
    {
        [SerializeField]
        private PassthroughSettingsSync passthroughSettingsSync;
        
        public void SetUserRepresentation(UserRepresentationType newRepresentationType)
        {
            passthroughSettingsSync.SetAvatarMode(newRepresentationType);
        }

        public void SetUserRepresentationAvatar() => SetUserRepresentation(UserRepresentationType.HeadOnly);
        public void SetUserRepresentationPrimitive() => SetUserRepresentation(UserRepresentationType.GeometricPrimitive);
        public void SetUserRepresentationIK() => SetUserRepresentation(UserRepresentationType.IK);
    }
}