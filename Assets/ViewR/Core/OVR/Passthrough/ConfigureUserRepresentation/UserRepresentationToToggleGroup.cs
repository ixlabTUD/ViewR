using System;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.StatusManagement.Listeners;
using ViewR.StatusManagement.States;

namespace ViewR.Core.OVR.Passthrough.ConfigureUserRepresentation
{
    public class UserRepresentationToToggleGroup : UserRepresentationListener
    {
        [SerializeField]
        private ToggleDeselect toggleAvatar;

        [FormerlySerializedAs("toggleCapsule")]
        [SerializeField]
        private ToggleDeselect togglePrimitive;

        [SerializeField]
        private ToggleDeselect toggleIK;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!Application.isPlaying)
                return;

            UpdateVisuals();
        }

        protected override void HandleRepresentationChanges(UserRepresentationType newUserRepresentation)
        {
            base.HandleRepresentationChanges(newUserRepresentation);

            UpdateVisuals(newUserRepresentation);
        }

        private void UpdateVisuals(UserRepresentationType? userRepresentation = null)
        {
            // Catch if null:
            userRepresentation ??= UserRepresentation.CurrentUserRepresentationType;

            switch (userRepresentation)
            {
                case UserRepresentationType.HeadOnly:
                    toggleAvatar.Select();
                    toggleAvatar.isOn = true;
                    break;
                case UserRepresentationType.GeometricPrimitive:
                    togglePrimitive.Select();
                    togglePrimitive.isOn = true;
                    break;
                case UserRepresentationType.IK:
                    toggleIK.Select();
                    toggleIK.isOn = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(userRepresentation), userRepresentation, null);
            }
        }
    }
}