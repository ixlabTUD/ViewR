using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.Visibility;
using ViewR.HelpersLib.Extensions.General;
using ViewR.HelpersLib.Utils.ToggleObjects;
using ViewR.StatusManagement;
using ViewR.StatusManagement.States;

namespace ViewR.Core.Networking.Normcore.Avatar
{
    /// <summary>
    /// Class that manages how this avatar is displayed.
    /// This is not synced, but based on the users <see cref="ClientPhysicalLocationState"/>, the configured <see cref="UserAvatarVideoVisibility"/> and the configured <see cref="UserRepresentation.CurrentUserRepresentationType"/>
    ///
    /// If the user itself is remote, they should see every user as an avatar.
    /// If the user is on site, they should see remote users as an avatar.
    /// If the user is on site, they should see co-located users according to <see cref="UserAvatarVideoVisibility"/> and <see cref="UserRepresentationType"/>
    /// </summary>
    public class AvatarVisualRepresentationManager : MonoBehaviour
    {
        [Header("Avatar")]
        [SerializeField]
        private ObjectsToToggle avatarObjects;

        [Header("Simple Shape")]
        [FormerlySerializedAs("capsuleObjects")]
        [SerializeField]
        private ObjectsToToggle simplifiedShapeObjects;

        [Header("References")]
        [SerializeField]
        private AvatarAccessHelper avatarAccessHelper;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        #region unity methods

        private void OnEnable()
        {
            // Subscribe
            avatarAccessHelper.SyncedPlayerPropertiesSync.PhysicalLocationDidChange += HandleLocationChanges;
            UserAvatarVideoVisibility.UserVideoVisibilityDidChange += HandleUserVideoVisibilityChanges;
            UserRepresentation.CurrentUserRepresentationDidChange += HandleNewUserRepresentation;

            if (debugging)
                Debug.Log($"{nameof(AvatarVisualRepresentationManager)}.{nameof(OnEnable)}: Calling NewConfiguration!", this);

            // Initial run.
            // Might cause errors as no realtime model is present yet.
            try
            {
                NewConfiguration();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Running {nameof(NewConfiguration)} with {nameof(DelayedRunner)}, because {nameof(NewConfiguration)} failed with:\n" + e);
                
                StartCoroutine(DelayedRunner());
            }
        }

        private IEnumerator DelayedRunner()
        {
            yield return new WaitForSeconds(2);

            if (debugging)
                Debug.Log($"Delayed: Calling NewConfiguration delayed!".StartWithFrom(GetType()), this);

            NewConfiguration();
        }

        private void OnDisable()
        {
            // Unsubscribe
            avatarAccessHelper.SyncedPlayerPropertiesSync.PhysicalLocationDidChange -= HandleLocationChanges;
            UserAvatarVideoVisibility.UserVideoVisibilityDidChange -= HandleUserVideoVisibilityChanges;
            UserRepresentation.CurrentUserRepresentationDidChange -= HandleNewUserRepresentation;
        }

        #endregion

        #region Callbacks to events

        /// <summary>
        /// Changes based on physical location
        /// </summary>
        /// <param name="value"></param>
        private void HandleLocationChanges(ClientPhysicalLocation value)
        {
            NewConfiguration();
        }

        /// <summary>
        /// Changes based on configuration changes via <see cref="UserAvatarVideoVisibility"/>
        /// </summary>
        private void HandleUserVideoVisibilityChanges(bool previousValue, bool newVisibleValue)
        {
            NewConfiguration();
        }

        private void HandleNewUserRepresentation(UserRepresentationType newUserRepresentationType)
        {
            NewConfiguration();
        }

        #endregion

        #region Avatar configuration logic

        private void NewConfiguration()
        {
            if (debugging)
                Debug.Log("Requested New configuration.".StartWithFrom(GetType()), this);

            // if this == our avatar -> hide and return
            if (avatarAccessHelper.RealtimeAvatar.realtimeView.isOwnedLocallySelf)
            {
                if (debugging)
                    Debug.Log(
                        "HideAvatarCompletely <- avatarAccessHelper.RealtimeAvatar.isOwnedLocallySelf.".StartWithFrom(
                            GetType()), this);
                HideAvatarCompletely();
                return;
            }

            // If we are locally remote: show avatars only.
            if (ClientPhysicalLocationState.CurrentClientPhysicalLocation == ClientPhysicalLocation.Remote)
            {
                if (debugging)
                    Debug.Log("ShowAvatar <- LocalUser is Remote.".StartWithFrom(GetType()), this);
                ShowAvatar();
                return;
            }

            // If we are on site:
            // If received remote from this avatar: show avatar.
            if (avatarAccessHelper.SyncedPlayerPropertiesSync.GetCurrentPhysicalLocation() ==
                ClientPhysicalLocation.Remote)
            {
                if (debugging)
                    Debug.Log("ShowAvatar <- IncomingAvatar is Remote.".StartWithFrom(GetType()), this);
                ShowAvatar();
                return;
            }

            //! So, we are on site and so is this avatar.
            // If we have configured the video to not be shown:
            if (!UserAvatarVideoVisibility.Visible)
            {
                if (debugging)
                    Debug.Log("ShowAvatar <- UserVideoVisibility = false.".StartWithFrom(GetType()), this);
                ShowAvatar();
                return;
            }
            // Else: Show according to UserRepresentation
            else
            {
                // Show it according to the UserRepresentation configuration
                switch (UserRepresentation.CurrentUserRepresentationType)
                {
                    case UserRepresentationType.HeadOnly:
                        if (debugging)
                            Debug.Log(
                                $"ShowAvatar <- CurrentUserRepresentationType = {UserRepresentationType.HeadOnly}."
                                    .StartWithFrom(GetType()), this);
                        ShowAvatar();
                        break;
                    case UserRepresentationType.GeometricPrimitive:
                        if (debugging)
                            Debug.Log(
                                $"ShowPassthroughCapsule <- CurrentUserRepresentationType = {UserRepresentationType.GeometricPrimitive}."
                                    .StartWithFrom(GetType()), this);
                        ShowPassthroughCapsule();
                        break;
                    case UserRepresentationType.IK:
                        if (debugging)
                            Debug.Log(
                                $"ShowPassthroughIK <- CurrentUserRepresentationType = {UserRepresentationType.IK}."
                                    .StartWithFrom(GetType()), this);
                        ShowPassthroughIK();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #region Methods actually setting the visual representation

        private void HideAvatarCompletely()
        {
            if (debugging)
                Debug.Log($"Executing: {nameof(HideAvatarCompletely)}.".StartWithFrom(GetType()), this);

            avatarObjects.Enable(false);
            simplifiedShapeObjects.Enable(false);
        }

        private void ShowAvatar()
        {
            if (debugging)
                Debug.Log($"Executing: {nameof(ShowAvatar)}.".StartWithFrom(GetType()), this);

            avatarObjects.Enable(true);
            simplifiedShapeObjects.Enable(false);
        }

        private void ShowPassthroughCapsule()
        {
            if (debugging)
                Debug.Log($"Executing: {nameof(ShowPassthroughCapsule)}.".StartWithFrom(GetType()), this);

            avatarObjects.Enable(false);
            simplifiedShapeObjects.Enable(true);
        }

        /// <summary>
        /// Shows the avatar in with passthrough IK
        /// </summary>
        private void ShowPassthroughIK()
        {
            if (debugging)
                Debug.Log($"Executing: {nameof(ShowPassthroughIK)}.".StartWithFrom(GetType()), this);

            /*
            Debug.LogWarning(
                "Please import the FinalIK-VRIK package and set the appropriate flags. Showing capsule instead.",
                this);
            ShowPassthroughCapsule();
            */
        }

        #endregion

        #endregion
    }
}